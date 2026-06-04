// Create OrderHeader
const sendData = document.querySelector(".enterData");
const shipCity = document.querySelector("#shipCity");
const shipAddress = document.querySelector("#shipAddress");

// Select Customer Modal
const productList = document.getElementById("modalProductTableBody");
const customerNameDisplay = document.querySelector(".customerNameDisplay");
const customerPhoneDisplay = document.querySelector(".customerPhoneDisplay");
const customerSelectionBtn = document.querySelector(".customerSelectionBtn");

// Select Product Modal
const modalTableBody = document.getElementById("modalTableBody");
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
let productsCache = []; 

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
};

// Ajax : Fetch All Products 
const fetchProducts = async () => {

    try {

        const response = await fetch(`${baseUrl}/Product/GetAll`);

        const data = await response.json();

        productsCache = data;

        showProducts(data);

    } catch (error) {

        console.error(error);

    }

}


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
};

// Show All Products
const showProducts = products => {

    productList.innerHTML = "";

    products.forEach(product => {

     
        const isAlreadyAdded = orderDetails.some(item => item.productID === product.productID);

       
        const disabledAttr = isAlreadyAdded ? 'disabled' : '';
        const buttonClass = isAlreadyAdded ? 'btn btn-secondary btn-sm' : 'btn btn-success btn-sm';
        const buttonText = isAlreadyAdded ? 'قبلاً اضافه شده' : 'انتخاب';

        productList.insertAdjacentHTML("beforeend",
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
};

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

// RemoveOrderDetail
const removeInvoiceItem = (orderDetailID) => {

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
            alert("سفارش با موفقیت ثبت شد");
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
        alert("خطا در ثبت سفارش");
    }
};

const btnOpenModal = document.querySelector('[data-bs-target="#customerModal"]');
btnOpenModal.addEventListener('click', () => {
    fetchCustomers();
});

const btnOpenProductModal = document.querySelector('[data-bs-target="#selectProductModal"]');
btnOpenProductModal.addEventListener('click', () => {
    fetchProducts(); 
});

sendData.addEventListener("click", create);
customerSelectionBtn.addEventListener("click", fetchCustomers);
productSelectionBtn.addEventListener("click", fetchProducts);