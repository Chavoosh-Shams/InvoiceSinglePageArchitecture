// Create Modal Elements 
const productNameInput = document.querySelector("#productNameInput");
const unitPriceInput = document.querySelector("#unitPriceInput");
const createBtn = document.querySelector(".create-btn");

// Update Modal Elements 
const productNameUpdateModal = document.querySelector(".productName-update-modal");
const unitPriceUpdateModal = document.querySelector(".unitPrice-update-modal");
const updateBtn = document.querySelector(".update-btn");

// Delete Modal Element
const removeBtn = document.querySelector(".remove-btn");

// Details Modal Elements 
const detailId = document.querySelector("#detailId");
const detailProductName = document.querySelector("#detailProductName");
const detailUnitPrice = document.querySelector("#detailUnitPrice");

// Count 
const productCount = document.querySelector("#count");

// Table Body Elem For Inserting Html Records
const tableBody = document.querySelector(".table-body");

// Global Variables
let selectedId = null;

// Base URL for AJAX Fetch Requests
const baseUrl = "http://localhost:5013/Product";

// Ajax : Get All
const fetchProducts = async () => {

    try {

        const response = await fetch(`${baseUrl}/GetAll`);

        const data = await response.json();

        showProducts(data);

    } catch (error) {

        console.error(error);

    }

}

// Render Last Three products
const showProducts = products => {

    tableBody.innerHTML = "";

    productCount.innerHTML = `There are ${products.length} products in DataBase`;

    const lastproducts = products.slice(-3);

    lastproducts.forEach(product => {

        tableBody.insertAdjacentHTML("beforeend",
            `
                <tr>
                    <td>${product.productName}</td>
                    <td>${product.unitPrice}</td>
                    <td>
                        <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deleteModal" onclick="setId('${product.productID}')">Remove</button>
                        <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#updateModal" onclick="populateUpdateModal('${product.productID}','${product.productName}','${product.unitPrice}')">Update</button>
                        <button class="btn btn-info text-white" data-bs-toggle="modal" data-bs-target="#detailsModal" onclick="showDetail('${product.productID}')">Details</button>
                    </td>
                </tr>
            `
        );

    });
}

// Ajax : Create 
const create = async () => {

    try {

        const validation = validateProduct(
            productNameInput.value,
            unitPriceInput.value
        );

        if (!validation.isValid) {
            alert(validation.message);
            return;
        }

        const newProduct = {
            productID: crypto.randomUUID(),
            productName: productNameInput.value.trim(),
            unitPrice: unitPriceInput.value.trim(),
        };

        const response = await fetch(`${baseUrl}/Post`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(newProduct),
        });

        if (response.ok) {

            const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createModal"));

            modal.hide();

            fetchProducts();

        } else {

            throw new Error("Request failed");

        }

        inputCleaner();

    } catch (error) {

        console.error(error);

    }

}

// Ajax : Update
const update = async () => {

    try {

        const validation = validateProduct(
            productNameUpdateModal.value,
            unitPriceUpdateModal.value,
        );

        if (!validation.isValid) {
            alert(validation.message);
            return;
        }

        const updateproduct = {
            productID: selectedId,
            productName: productNameUpdateModal.value.trim(),
            unitPrice: unitPriceUpdateModal.value.trim(),
        };

        const response = await fetch(`${baseUrl}/Put`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(updateproduct),
        });

        if (response.ok) {

            const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById("updateModal"));

            modal.hide();

            fetchProducts();

        } else {

            throw new Error("Request failed");

        }

    } catch (error) {

        console.error(error);

    }

}

// Ajax : Remove
const remove = async () => {

    try {

        const response = await fetch(`${baseUrl}/Delete`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify({
                productID: selectedId
            })
        });

        if (response.ok) {

            const modalElement = document.getElementById("deleteModal");

            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);

            modal.hide();

            fetchProducts();

        }

    }
    catch (error) {

        console.error(error);

    }

}

// Ajax : Get By Id
const showDetail = async (productID) => {

    try {

        const response = await fetch(`${baseUrl}/GetById?productID=${productID}`);

        const product = await response.json();

        detailId.innerHTML = product.productID;

        detailProductName.innerHTML = product.productName;

        detailUnitPrice.innerHTML = product.unitPrice;

    } catch (error) {

        console.error(error);

    }

}

// Functions: Save ID for Edit or Remove
const setId = Id => {
    selectedId = Id;
}

// Functions: Populate Update Modal Inputs and Store Selected ID
const populateUpdateModal = (productId, productName, unitPrice) => {
    setId(productId);
    productNameUpdateModal.value = productName;
    unitPriceUpdateModal.value = unitPrice;
}

// Functions: Clear Form Inputs 
const inputCleaner = () => {

    productNameInput.value = "";

    unitPriceInput.value = "";

}

// Validation for Product
const validateProduct = (productName, unitPrice) => {


    const nameRegex = /^[a-zA-Zآ-ی0-9\s]+$/;

    productName = (productName || "").trim();

    const priceValue = parseFloat(unitPrice);

    if (!productName || unitPrice === undefined || unitPrice === null || unitPrice === "") {
        return {
            isValid: false,
            message: "Product name and unit price are required"
        };
    }

    if (productName.length < 3) {
        return {
            isValid: false,
            message: "Product name must be at least 3 characters"
        };
    }

    if (!nameRegex.test(productName)) {
        return {
            isValid: false,
            message: "Product names can only contain letters and numbers"
        };
    }

    if (isNaN(priceValue)) {
        return {
            isValid: false,
            message: "Unit price must be a valid number"
        };
    }

    if (priceValue <= 0) {
        return {
            isValid: false,
            message: "Unit price must be greater than zero"
        };
    }

    return {
        isValid: true
    };
}

// Event Listeners
window.addEventListener("load", fetchProducts);
createBtn.addEventListener("click", create);
updateBtn.addEventListener("click", update);
removeBtn.addEventListener("click", remove);