// Create Modal Elements 
const sendData = document.querySelector(".enterData");
const shipCity = document.querySelector("#shipCity");
const shipAddress = document.querySelector("#shipAddress");

// Select Customer Modal
const productsTableBodyElem = document.querySelector("#modalProductTableBody");
const customerNameDisplay = document.querySelector(".customerNameDisplay");
const customerPhoneDisplay = document.querySelector(".customerPhoneDisplay");
const customerSelectionBtn = document.querySelector(".customerSelectionBtn");

// Select Product Modal
const modalTableBody = document.querySelector("#modalTableBody");
const productNameField = document.querySelector(".productNameField");
const productCountField = document.querySelector(".productCountField");
const productUnitPriceField = document.querySelector(".productUnitPriceField");
const productSelectionBtn = document.querySelector(".productSelectionBtn");

// Table Body
const invoiceBody = document.querySelector(".invoiceBody");

// Global Variables
let selectedId = null;
let CustomerID = null;
let orderDetails = [];

// Base URL
const baseUrl = "http://localhost:5013";

// Ajax : Fetch All Customers 
const fetchCustomers = async () => {

    try {

        const response = await fetch(`${baseUrl}/Customer/GetAll`);

        const data = await response.json();

        showCustomers(data);

    } catch (error) {

        console.error(error);

    }
}

// Show All Customers 
const showCustomers = customers => {

    modalTableBody.innerHTML = "";

    customers.forEach(customer => {

        modalTableBody.insertAdjacentHTML("beforeend",

            `

                <tr>
                    <td>${customer.firstName}</td>
                    <td>${customer.lastName}</td>
                    <td>${customer.phone}</td>
                    <td>${customer.city}</td>
                    <td>${customer.address}</td>
                    <td>
                        <button class="btn btn-success btn-sm" 
                                onclick="selectCustomer('${customer.customerID}', '${customer.firstName}', '${customer.lastName}', '${customer.phone}')">
                                Select
                        </button>
                    </td>
                </tr>

            `
        );
    });
}

// Selected Customer Info
const selectCustomer = (customerId, firstName, lastName, phone) => {
    CustomerID = customerId;
    console.log(CustomerID)
    customerNameDisplay.innerHTML = `${firstName} ${lastName}`;
    customerPhoneDisplay.innerHTML = phone;
    const modalElement = document.getElementById('customerModal');
    const modal = bootstrap.Modal.getInstance(modalElement);
    modal.hide();
}

// Ajax : Fetch All Products 
const fetchProducts = async () => {

    try {

        const response = await fetch(`${baseUrl}/Product/GetAll`);

        const data = await response.json();

        showProducts(data);

    } catch (error) {

        console.error(error);

    }

}

// Render Order Details
const showOrderDetailItems = () => {

    invoiceBody.innerHTML = "";

    let subTotal = 0;
    
    orderDetails.forEach((item, index) => {

        const rowTotal = item.unitPrice * item.quantity;

        subTotal += rowTotal;

        invoiceBody.insertAdjacentHTML("beforeend",
            `
            <tr>
                <td>${index + 1}</td>
                <td>${item.productName}</td>
                <td>${item.quantity}</td>
                <td>${item.unitPrice.toLocaleString()}</td>
                <td>${rowTotal.toLocaleString()}</td>
                <td>
                    <button
                        class="btn btn-danger btn-sm"
                        onclick="removeInvoiceItem('${item.orderDetailID}')">
                        Delete
                    </button>
                </td>
            </tr>
            `
        );
    });

    // محاسبات مالی
    const tax = subTotal * 0.09; // 9 درصد مالیات
    const finalPrice = subTotal + tax;

    document.querySelector("#totalPrice").textContent =
        subTotal.toLocaleString();

    document.querySelector("#taxPrice").textContent =
        tax.toLocaleString();

    document.querySelector("#finalPrice").textContent =
        finalPrice.toLocaleString();
}

// Render Products List
const showProducts = products => {

    productsTableBodyElem.innerHTML = "";

    products.forEach(product => {

        const isAdded = orderDetails.some(
            item => item.productID === product.productID
        );

        const rowClass = isAdded ? "table-secondary opacity-50" : "";
        const buttonClass = isAdded
            ? "btn btn-secondary btn-sm"
            : "btn btn-success btn-sm";

        const buttonText = isAdded
            ? "Already Added"
            : "Select";

        const disabled = isAdded ? "disabled" : "";

        productsTableBodyElem.insertAdjacentHTML(
            "beforeend",
            `
            <tr class="${rowClass}">
                <td>${product.productName}</td>
                <td>${Number(product.unitPrice).toLocaleString()}</td>
                <td>
                    <input
                        type="number"
                        class="form-control form-control-sm text-center productCountInput"
                        value="1"
                        min="1"
                        ${disabled}
                    >
                </td>
                <td>
                    <button
                        type="button"
                        class="${buttonClass}"
                        ${disabled}
                        onclick="selectProduct(this, '${product.productID}', '${product.productName}', ${product.unitPrice})">
                        ${buttonText}
                    </button>
                </td>
            </tr>
            `
        );
    });
}

// Add Product To Invoice
const selectProduct = (button, productId, productName, unitPrice) => {

    // Prevent duplicate products
    const exists = orderDetails.some(item => item.productID === productId);

    if (exists) {
        alert(`Product "${productName}" has already been added to this order.`);
        return;
    }

    const quantityInput = button
        .closest("tr")
        .querySelector(".productCountInput");

    const quantity = Number(quantityInput.value);

    if (!quantity || quantity <= 0) {
        alert("Please enter a valid quantity.");
        return;
    }

    const orderDetail = {
        orderDetailID: crypto.randomUUID(),
        productID: productId,
        productName,
        unitPrice: Number(unitPrice),
        quantity
    };

    orderDetails.push(orderDetail);

    showOrderDetailItems();
}

// RemoveOrderDetail
const removeInvoiceItem = orderDetailID => {

    const index = orderDetails.findIndex(orderDetail => orderDetail.orderDetailID === orderDetailID);
    if (index !== -1) {
        orderDetails.splice(index, 1);
    }
    showOrderDetailItems();
}

// Ajax : Create
const create = async () => {

    try {

        const orderDateValue = document.querySelector("#orderDate").value;


        if (!orderDateValue) {
            alert("Please select an order date.");
            return;
        }

        if (!CustomerID) {
            alert("Please select a customer.");
            return;
        }

        if (orderDetails.length === 0) {
            alert("At least one product must be selected.");
            return;
        }

        const orderId = crypto.randomUUID();

        const newOrder = {
            orderHeaderID: orderId,
            orderDate: orderDateValue + "T00:00:00",
            shipCity: shipCity.value || "",
            shipAddress: shipAddress.value || "",
            customerID: CustomerID,
            postOrderDetailDtos: orderDetails.map(item => ({
                orderDetailID: item.orderDetailID,
                orderHeaderID: orderId,
                productID: item.productID,
                unitPrice: Number(item.unitPrice),
                quantity: Number(item.quantity)
            }))
        };

        console.log("FINAL JSON:", newOrder);

        const response = await fetch(`${baseUrl}/OrderHeader/Post`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(newOrder),
        });

        const text = await response.text();

        console.log("STATUS:", response.status);
        console.log("RESPONSE:", text);

        if (response.ok) {

            alert("Order updated successfully.");

            orderDetails = [];

            CustomerID = null;

            customerNameDisplay.innerHTML = "";

            customerPhoneDisplay.innerHTML = "";

            shipCity.value = "";

            shipAddress.value = "";

            showOrderDetailItems();

        }

    } catch (err) {

        console.error(err);

        alert("Error while registering the order.");

    }
}

const btnOpenModal = document.querySelector('[data-bs-target="#customerModal"]');
btnOpenModal.addEventListener('click', () => {
    fetchCustomers();
});

const btnOpenProductModal = document.querySelector('[data-bs-target="#selectProductModal"]');
btnOpenProductModal.addEventListener('click', () => {
    fetchProducts(); 
});

// Event Listeners
sendData.addEventListener("click", create);
customerSelectionBtn.addEventListener("click", fetchCustomers);
productSelectionBtn.addEventListener("click", fetchProducts);