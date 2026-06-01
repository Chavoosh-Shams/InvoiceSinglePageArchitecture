// Create Modal Elements 
const firstNameInput = document.querySelector("#firstNameInput");
const lastNameInput = document.querySelector("#lastNameInput");
const phoneNumberInput = document.querySelector("#phoneNumberInput");
const cityInput = document.querySelector("#cityInput");
const addressInput = document.querySelector("#addressInput");
const createBtn = document.querySelector(".create-btn");

// Update Modal Elements 
const firstNameUpdateModalInput = document.querySelector(".firstName-update-modal");
const lastNameUpdateModalInput = document.querySelector(".lastName-update-modal");
const phoneUpdateModalInput = document.querySelector(".phone-update-modal");
const cityUpdateModalInput = document.querySelector(".city-update-modal");
const addressUpdateModalInput = document.querySelector(".address-update-modal");
const updateBtn = document.querySelector(".update-btn");

// Delete Modal Element
const removeBtn = document.querySelector(".remove-btn");

// Details Modal Elements 
const detailId = document.querySelector("#detailId");
const detailFirstName = document.querySelector("#detailFirstName");
const detailLastName = document.querySelector("#detailLastName");
const detailPhone = document.querySelector("#detailPhone");
const detailCity = document.querySelector("#detailCity");
const detailAddress = document.querySelector("#detailAddress");

// Count 
const customerCount = document.querySelector("#count");

// Table Body Elem For Inserting Html Records
const tableBody = document.querySelector(".table-body");

// Global Variables
let selectedId = null;

// Base URL for AJAX Fetch Requests
const baseUrl = "http://localhost:5013/Customer";

// Ajax : Fetch All Customers 
const fetchCustomers = async () => {

    try {

        const response = await fetch(`${baseUrl}/GetAll`);

        const data = await response.json();

        showCustomers(data);

    } catch (error) {

        console.error(error);

    }
}

// Render Last Three Customers
const showCustomers = customers => {

    tableBody.innerHTML = "";

    customerCount.innerHTML = `There are ${customers.length} Customers in DataBase`;

    const lastCustomers = customers.slice(-3);

    lastCustomers.forEach(customer => {

        tableBody.insertAdjacentHTML("beforeend",
            `
                <tr>
                    <td>${customer.firstName}</td>
                    <td>${customer.lastName}</td>
                    <td>${customer.phone}</td>
                    <td>${customer.city}</td>
                    <td>${customer.address}</td>
                    <td>
                        <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#deleteModal" onclick="setId('${customer.customerID}')">Remove</button>
                        <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#updateModal" onclick="populateUpdateModal('${customer.customerID}','${customer.firstName}','${customer.lastName}','${customer.phone}','${customer.city}','${customer.address}')">Update</button>
                        <button class="btn btn-info text-white" data-bs-toggle="modal" data-bs-target="#detailsModal" onclick="showDetail('${customer.customerID}')">Details</button>
                    </td>
                </tr>
            `
        );

    })
}

// Ajax : Create 
const create = async () => {

    try {

        const validation = validationInput(
            firstNameInput.value,
            lastNameInput.value,
            phoneNumberInput.value,
            cityInput.value,
            addressInput.value,
        );

        if (!validation.isValid) {
            alert(validation.message);
            return;
        }

        // Generate a unique GUID
        const id = crypto.randomUUID();

        const newCustomer = {
            customerID: id,
            firstName: firstNameInput.value.trim(),
            lastName: lastNameInput.value.trim(),
            phone: phoneNumberInput.value.trim(),
            city: cityInput.value.trim(),
            address: addressInput.value.trim(),
        };

        const response = await fetch(`${baseUrl}/Post`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(newCustomer),
        });

        if (response.ok) {
            const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById("createModal"));
            modal.hide();
            fetchCustomers();

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

        const validation = validationInput(
            firstNameUpdateModalInput.value,
            lastNameUpdateModalInput.value,
            phoneUpdateModalInput.value,
            cityUpdateModalInput.value,
            addressUpdateModalInput.value,
        );

        if (!validation.isValid) {
            alert(validation.message);
            return;
        }

        const updateCustomer = {
            customerID: selectedId,
            firstName: firstNameUpdateModalInput.value.trim(),
            lastName: lastNameUpdateModalInput.value.trim(),
            phone: phoneUpdateModalInput.value.trim(),
            city: cityUpdateModalInput.value.trim(),
            address: addressUpdateModalInput.value.trim(),
        };

        const response = await fetch(`${baseUrl}/Put`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(updateCustomer),
        });

        if (response.ok) {
            const modal = bootstrap.Modal.getOrCreateInstance(document.getElementById("updateModal"));
            modal.hide();
            fetchCustomers();
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
                customerID: selectedId
            }),
        });

        if (response.ok) {

            const modalElement = document.getElementById("deleteModal");
            const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
            modal.hide();
            fetchCustomers();

        }
    }
    catch (error) {
        console.error(error);
    }
};

// Ajax : Get By Id
const showDetail = async (customerID) => {

    try {
        const response = await fetch(`${baseUrl}/GetById?CustomerID=${customerID}`);

        const customer = await response.json();
        console.log(customer)
        detailId.innerHTML = customer.customerID;
        detailFirstName.innerHTML = customer.firstName;
        detailLastName.innerHTML = customer.lastName;
        detailPhone.innerHTML = customer.phone;
        detailCity.innerHTML = customer.city;
        detailAddress.innerHTML = customer.address;

    } catch (error) {

        console.error(error);
    }

}

// Functions: Save ID for Edit or Remove
const setId = Id => {
    selectedId = Id;
}

// Functions: Populate Update Modal Inputs and Store Selected ID
const populateUpdateModal = (customerId, firstName, lastName, phone, city, address) => {
    setId(customerId);
    firstNameUpdateModalInput.value = firstName;
    lastNameUpdateModalInput.value = lastName;
    phoneUpdateModalInput.value = phone;
    cityUpdateModalInput.value = city;
    addressUpdateModalInput.value = address;
}

// Functions: Clear Form Inputs 
const inputCleaner = () => {
    firstNameInput.value = "";
    lastNameInput.value = "";
    phoneNumberInput.value = "";
    cityInput.value = "";
    addressInput.value = "";
}

// Validation
const validationInput = (firstName, lastName, phone, city, address) => {

    const nameRegex = /^[a-zA-Zآ-ی\s]+$/;

    const phoneRegex = /^(?:0|\+?98)?9\d{9}$/;

    const cityRegex = /^[a-zA-Zآ-ی\s]+$/;

    firstName = (firstName || "").trim();
    lastName = (lastName || "").trim();
    phone = (phone || "").trim();
    city = (city || "").trim();
    address = (address || "").trim();

    if (!firstName || !lastName || !phone || !city || !address) {
        return {
            isValid: false,
            message: "تمامی فیلدها (نام، نام خانوادگی، تلفن، شهر، آدرس) الزامی هستند"
        };
    }

    if (firstName.length < 2 || lastName.length < 2) {
        return {
            isValid: false,
            message: "نام و نام خانوادگی باید حداقل ۲ کاراکتر باشند"
        };
    }


    if (!nameRegex.test(firstName) || !nameRegex.test(lastName)) {
        return {
            isValid: false,
            message: "فقط حروف فارسی/انگلیسی و فاصله برای نام و نام خانوادگی مجاز هستند"
        };
    }

    if (city.length < 2) {
        return {
            isValid: false,
            message: "نام شهر باید حداقل ۲ کاراکتر باشد"
        };
    }
    if (!cityRegex.test(city)) {
        return {
            isValid: false,
            message: "نام شهر فقط می‌تواند شامل حروف فارسی/انگلیسی و فاصله باشد"
        };
    }

    if (!phoneRegex.test(phone)) {
        return {
            isValid: false,
            message: "شماره تلفن معتبر نیست (مثال صحیح: 09121234567 یا +989121234567)"
        };
    }

    if (address.length < 5) {
        return {
            isValid: false,
            message: "آدرس باید حداقل ۵ کاراکتر باشد"
        };
    }

    return {
        isValid: true
    };
}

// Event Listeners
window.addEventListener("load", fetchCustomers);
createBtn.addEventListener("click", create);
updateBtn.addEventListener("click", update);
removeBtn.addEventListener("click", remove);