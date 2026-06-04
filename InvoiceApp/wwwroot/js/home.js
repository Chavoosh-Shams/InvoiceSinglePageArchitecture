// All OrderHeaders Elems
const invoiceTableBody = document.querySelector("#invoiceTableBody");
const removeBtn = document.querySelector(".remove-btn");
const customerModal = document.getElementById("customerModal");
const productModal = document.getElementById("selectProductModal");
const customerTableBodyElem = document.querySelector("#customerTableBody");

// Update Modal
const customerNameDisplay = document.querySelector(".customerNameDisplay");
const customerPhoneDisplay = document.querySelector(".customerPhoneDisplay");
const updateShipCityInput = document.querySelector("#updateShipCity");
const updateShipAddressInput = document.querySelector("#updateShipAddress");
const customerSelectBtn = document.querySelector(".Customer-Select-Btn");
const productSelectBtn = document.querySelector(".Product-Select-Btn");
const productsTableBodyElem = document.querySelector("#Products-Table-Body");
const orderDetailTableBodyElem = document.querySelector(".orderDetailBody");
const saveUpdateOrderBtn = document.querySelector("#saveUpdateOrderBtn");

//Info Modal
const invoiceInfoBody = document.querySelector(".invoiceInfoBody");
const customerNameDisplayInfo = document.querySelector(".customerNameDisplayInfo");
const customerPhoneDisplayInfo = document.querySelector(".customerPhoneDisplayInfo");
const orderDateInfo = document.querySelector("#orderDateInfo");
const shipCityInfo = document.querySelector("#shipCityInfo");
const shipAddressInfo = document.querySelector("#shipAddressInfo");

// Global Variables
let selectedId = null;
let CustomerID = null;
let selectedOrderHeader = null;
let invoiceItems = [];
let orderDetails = [];

// Base URL
const baseUrl = "http://localhost:5013/OrderHeader";

// Ajax : Fetch All OrderHeaders 
const fetchOrderHeader = async () => {

    try {

        const res = await fetch(`${baseUrl}/GetAll`);

        const data = await res.json();

        renderOrders(data);

    } catch (err) {

        console.error(err);

    }
}

// Render All OrderHeaders 
const renderOrders = (orders) => {
    invoiceTableBody.innerHTML = "";

    orders.forEach((o, index) => {
        invoiceTableBody.insertAdjacentHTML("beforeend",
            `
            <tr>
                <td>${index + 1}</td>
                <td><span class="badge bg-dark">${o.orderHeaderID}</span></td>
                <td>${o.customerFirstName} ${o.customerLastName}</td>
                <td>${o.orderDate}</td>
                <td>${o.shipCity}</td>
                <td>${o.shipAddress}</td>
                <td>

                    <button class="btn btn-info btn-sm text-white"
                        data-bs-toggle="modal"
                        data-bs-target="#infoModal"
                        onclick="openInfoModal('${o.orderHeaderID}')">
                        Info
                    </button>

                    <button class="btn btn-warning btn-sm text-white"
                        data-bs-toggle="modal"
                        data-bs-target="#updateModal"
                        onclick="openUpdateModal('${o.orderHeaderID}')">
                        Edite
                    </button>

                    <button class="btn btn-danger btn-sm"
                        data-bs-toggle="modal"
                        data-bs-target="#deleteModal"
                        onclick="setDeleteId('${o.orderHeaderID}')">
                        Delete
                    </button>

                </td>
            </tr>
        `
        );
    });
}

// Open Update Modal
const openUpdateModal = async (orderHeaderId) => {
    selectedOrderHeader = orderHeaderId
    await fetchOrderDetailsForUpdate(orderHeaderId);
}

// Open Info Modal
const openInfoModal = async (orderHeaderId) => {
    await fetchOrderHeaderForInfo(orderHeaderId);
};

// Ajax : Fetch OrderDetails By OrderHeader
const fetchOrderDetailsForUpdate = async (orderHeaderId) => {
    try {
        orderDetails = [];
        const url = `${baseUrl}/GetById?orderHeaderID=${orderHeaderId}`;

        const res = await fetch(url);

        if (!res.ok) {
            console.error("HTTP ERROR:", res.status);
            return;
        }

        const data = await res.json();

        console.log("DATA:", data);

        invoiceItems = data.getOrderDetails;

        invoiceItems.forEach(invoice => {
            orderDetails.push(invoice)
        })

        console.log(orderDetails);
        
        orderDetailTableBodyElem.innerHTML = "";

        updateShipCityInput.value = data.shipCity;

        updateShipAddressInput.value = data.shipAddress;

        customerNameDisplay.innerHTML = `${data.customerFirstName} ${data.customerLastName}`;

        customerPhoneDisplay.innerHTML = data.customerPhone;

        CustomerID = data.customerID;

        showOrderDetailItems();

    } catch (err) {

        console.error("OrderDetail Error:", err);

    }
}

// Render OrderDetails
const showOrderDetailItems = () => {
    orderDetailTableBodyElem.innerHTML=""
    orderDetails.forEach((item, index) => {
        console.log(item.productName)
        orderDetailTableBodyElem.insertAdjacentHTML("beforeend",
            `
            <tr>
                <td>${index + 1}</td>
                <td>${item.productName}</td>
                <td>${item.quantity}</td>
                <td>${item.unitPrice.toLocaleString()}</td>
                <td>${item.unitPrice.toLocaleString()}</td>
               
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
}

// Ajax : Fetch fetchOrderHeaderForInfo By OrderHeader
const fetchOrderHeaderForInfo = async (orderHeaderId) => {
    try {
        const url = `${baseUrl}/GetById?orderHeaderID=${orderHeaderId}`;

        const res = await fetch(url);

        if (!res.ok) {
            console.error("HTTP ERROR:", res.status);
            return;
        }

        const data = await res.json();

        console.log("DATA:", data);

        invoiceItems = data.getOrderDetails;

        invoiceInfoBody.innerHTML = "";

        invoiceItems.forEach((invoiceItem, index) => {
            invoiceInfoBody.insertAdjacentHTML("beforeend",
                `
                    <tr>
                        <td>${index + 1}</td>
                        <td>${invoiceItem.productName}</td>
                        <td>${invoiceItem.quantity}</td>
                        <td>${invoiceItem.unitPrice.toLocaleString()}</td>
                        <td>${invoiceItem.quantity * invoiceItem.unitPrice}</td>
                        <td>
                        
                        </td>
                    </tr>
                `);
        });
        orderDateInfo.value = data.orderDate;
        shipCityInfo.value = data.shipCity;
        shipAddressInfo.value = data.shipAddress;
        customerNameDisplayInfo.innerText = `${data.customerFirstName} ${data.customerFirstName}`;
        customerPhoneDisplayInfo.innerHTML = data.customerPhone;
    } catch (err) {
        console.error("OrderDetail Error:", err);
    }
}

// RemoveOrderDetail
const removeInvoiceItem = (orderDetailID) => {

    const index = orderDetails.findIndex(orderDetail => orderDetail.orderDetailID === orderDetailID);
    if (index !== -1) {
        orderDetails.splice(index, 1);
    }
    showOrderDetailItems();
}

// Ajax: Remove OrderHeader
const remove = async () => {
    try {
        if (!selectedId) return;

        const res = await fetch(`${baseUrl}/Delete`, {
            method: "DELETE",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ orderHeaderID: selectedId })
        });

        if (res.ok) {
            bootstrap.Modal.getInstance(document.getElementById("deleteModal")).hide();
            fetchOrderHeader();
        }
    } catch (err) {
        console.error(err);
    }
};

// Ajax : Fetch All Customers For Selecting In Updateing Order And Order Detail 
const fetchCustomers = async () => {

    try {

        const res = await fetch("http://localhost:5013/Customer/GetAll");

        const data = await res.json();

        showCustomers(data);

    } catch (err) {

        console.error(err);

    }
};

// Render All Customers
const showCustomers = customers => {

    try {

        customerTableBodyElem.innerHTML = "";

        customers.forEach(c => {

            customerTableBodyElem.insertAdjacentHTML("beforeend",
            `
                <tr>
                    <td>${c.firstName}</td>
                    <td>${c.lastName}</td>
                    <td>${c.phone}</td>
                    <td>${c.city}</td>
                    <td>${c.address}</td>
                    <td>
                        <button class="btn btn-success btn-sm"
                            onclick="selectCustomer('${c.customerID}','${c.firstName}','${c.lastName}','${c.phone}')">
                            Select
                        </button>
                    </td>
                </tr>
            `);

        });

    } catch (err) {

        console.error(err);

    }

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
};

// Ajax : Fetch All Products For Selecting In Updateing Order And Order Detail 
const fetchProducts = async () => {

    try {

        const response = await fetch("http://localhost:5013/Product/GetAll");

        const data = await response.json();

        productsCache = data;

        showProducts(data);

    } catch (error) {

        console.error(error);

    }

}

// Render Products List
const showProducts = (products) => {

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
};

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

// Ajax : Update
const update = async () => {

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

        const updateOrder = {
            orderHeaderID: selectedOrderHeader,
            orderDate: orderDateValue + "T00:00:00",
            shipCity: updateShipCityInput.value || "",
            shipAddress: updateShipAddressInput.value || "",
            customerID: CustomerID,
            putOrderDetailDtos: orderDetails.map(item => ({
                orderDetailID: item.orderDetailID,
                orderHeaderID: selectedOrderHeader,
                productID: item.productID,
                unitPrice: Number(item.unitPrice),
                quantity: Number(item.quantity)
            }))
        };

        console.log("FINAL JSON:", updateOrder);

        const response = await fetch(`${baseUrl}/Put`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(updateOrder),
        });

        const text = await response.text();

        console.log("STATUS:", response.status);
        console.log("RESPONSE:", text);

        if (response.ok) {

            alert("Order updated successfully.");

            const modalElement = document.getElementById("updateModal");
            const modal = bootstrap.Modal.getInstance(modalElement);
            modal.hide();

            orderDetails = [];
            CustomerID = null;

            customerNameDisplay.innerHTML = "";
            customerPhoneDisplay.innerHTML = "";

            updateShipCityInput.value = "";
            updateShipAddressInput.value = "";

            fetchOrderHeader();

        }

    } catch (err) {

        console.error(err);

        alert("Error while registering the order.");

    }
}

// Set ID
const setDeleteId = id => {
    selectedId = id;
}

// Open Modasl On The Another Modal
$(document).on('click', '#updateModal .Customer-Select-Btn', function () {
    const customerModal = new bootstrap.Modal('#customerModal');
    customerModal.show();
});

$(document).on('click', '#updateModal .Product-Select-Btn', function () {
    const productModal = new bootstrap.Modal('#selectProductModal');
    productModal.show();
});

// Event Listeners
window.addEventListener("load", fetchOrderHeader);
customerSelectBtn.addEventListener("click", fetchCustomers);
productSelectBtn.addEventListener("click", fetchProducts);
saveUpdateOrderBtn.addEventListener("click", update);
removeBtn.addEventListener("click", remove);