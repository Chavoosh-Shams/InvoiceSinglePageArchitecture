// 
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
let productsCache = []; 

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

        customerNameDisplay.innerHTML = `${data.customerFirstName} ${data.customerFirstName}`;

        customerPhoneDisplay.innerHTML = data.customerPhone;

        CustomerID = data.customerID;

        showOrderDetailItems();

    } catch (err) {

        console.error("OrderDetail Error:", err);

    }
}

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

const setDeleteId = (id) => {
    selectedId = id;
}

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

// Show All Products
const showProducts = products => {

    productsTableBodyElem.innerHTML = "";

    products.forEach(product => {


        const isAlreadyAdded = orderDetails.some(item => item.productID === product.productID);


        const disabledAttr = isAlreadyAdded ? 'disabled' : '';
        const buttonClass = isAlreadyAdded ? 'btn btn-secondary btn-sm' : 'btn btn-success btn-sm';
        const buttonText = isAlreadyAdded ? 'قبلاً اضافه شده' : 'انتخاب';

        productsTableBodyElem.insertAdjacentHTML("beforeend",
            `
                <tr class="${isAlreadyAdded ? 'table-secondary opacity-50' : ''}">
                    <td>${product.productName}</td>
                    <td>${product.unitPrice.toLocaleString()}</td>
                    <td>
                        <input type="number" class="form-control form-control-sm text-center productCountInput" value="1" min="1" ${disabledAttr}>
                     
                    <td>
                        <button class="${buttonClass}" ${disabledAttr}
                                onclick="if(!${isAlreadyAdded}) selectProduct(this, '${product.productID}', '${product.productName}', '${product.unitPrice}')">
                            ${buttonText}
                        </button>
                    </td>
                </tr>
            `
        );
    });
}

// Selected Product Info
const selectProduct = (button, productId, productName, unitPrice) => {

    const isDuplicate = orderDetails.some(item => item.productID === productId);

    if (isDuplicate) {
        alert(`❌ محصول "${productName}" قبلاً به این فاکتور اضافه شده است!`);
        return;
    }

    const count = button.closest("tr").querySelector(".productCountInput").value;

    const orderDetailId = crypto.randomUUID();

    const orderDetialObj = {
        orderDetailID: orderDetailId,
        productID: productId,
        productName: productName,
        unitPrice: Number(unitPrice),
        quantity: Number(count),
    }

    orderDetails.push(orderDetialObj);

    showOrderDetailItems();
}

// Ajax : Update
const update = async () => {

    try {

        const orderDateValue = document.querySelector("#orderDate").value;

        if (!orderDateValue) {
            alert("تاریخ انتخاب نشده");
            return;
        }

        if (!CustomerID) {
            alert("مشتری انتخاب نشده");
            return;
        }

        if (orderDetails.length === 0) {
            alert("حداقل یک محصول انتخاب کنید");
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

            alert("سفارش با موفقیت ثبت شد");

            orderDetails = [];

            CustomerID = null;

            customerNameDisplay.innerHTML = "";

            customerPhoneDisplay.innerHTML = "";

            updateShipCityInput.value = "";

            updateShipAddressInput.value = "";

            showOrderDetailItems();

        }

    } catch (err) {

        console.error(err);

        alert("خطا در ثبت سفارش");

    }
}

// Event Listeners
window.addEventListener("load", fetchOrderHeader);
customerSelectBtn.addEventListener("click", fetchCustomers);
productSelectBtn.addEventListener("click", fetchProducts);
saveUpdateOrderBtn.addEventListener("click", update);
removeBtn.addEventListener("click", remove);
$(document).on('click', '#updateModal .Customer-Select-Btn', function () {
    const customerModal = new bootstrap.Modal('#customerModal');
    customerModal.show();
});
$(document).on('click', '#updateModal .Product-Select-Btn', function () {
    const productModal = new bootstrap.Modal('#selectProductModal');
    productModal.show();
});