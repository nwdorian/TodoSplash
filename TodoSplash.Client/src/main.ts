import type { TodoRead } from "./models/todo-read";
import TodoService from "./services/todo-service";
import "./style.css";

const tableBody = document.getElementById("todos") as HTMLTableSectionElement;
const idInput = document.getElementById("edit-id") as HTMLInputElement;
const isCompleteInput = document.getElementById("edit-isComplete") as HTMLInputElement;
const nameInput = document.getElementById("edit-name") as HTMLInputElement;
const changesMessage = document.getElementById("changes-message") as HTMLParagraphElement;
const successMessage = document.getElementById("success-message") as HTMLParagraphElement;
const editForm = document.getElementById("edit-form") as HTMLDivElement;
const addInput = document.getElementById("add-input") as HTMLInputElement;
const errorAlert = document.getElementById("error-alert") as HTMLDivElement;
const saveButton = document.getElementById("btn-save-new") as HTMLButtonElement;
const saveUpdatesButton = document.getElementById("btn-save-update") as HTMLButtonElement;
const closeEditFormButton = document.getElementById("btn-close-edit") as HTMLButtonElement;

let todos: TodoRead[] = [];

const init = async () => {
    setupEventListeners();
    await refreshList();
};

const setupEventListeners = () => {
    saveButton.addEventListener("click", addItem);
    saveUpdatesButton.addEventListener("click", updateItem);
    closeEditFormButton.addEventListener("click", closeEditForm);
    nameInput.addEventListener("input", () => checkForChanges());
    isCompleteInput.addEventListener("change", () => checkForChanges());
};

const refreshList = async () => {
    const result = await TodoService.getAll();
    if (result.success && result.data) {
        todos = result.data;
        renderTable();
    } else {
        showError(result.error!);
    }
};

const showError = (message: string) => {
    let displayMessage = message;

    try {
        const parsed = JSON.parse(message) as { errors: string[] };

        if (parsed) {
            displayMessage = parsed.errors.join("");
        }
    } catch {}
    errorAlert.innerText = displayMessage;
    errorAlert.classList.remove("d-none");
};

const closeError = () => {
    errorAlert.classList.add("d-none");
};

const addItem = async () => {
    const name = addInput.value.trim();
    console.log(name);
    if (!name) {
        showError("Name can't be empty!");
        return;
    }

    const result = await TodoService.create({ name });
    if (result.success) {
        addInput.value = "";
        closeError();
        await refreshList();
    } else {
        showError(result.error!);
    }
};

const deleteItem = async (id: number) => {
    if (confirm("Are you sure?")) {
        const result = await TodoService.delete(id);
        if (result.success) {
            closeError();
            await refreshList();
        } else {
            showError(result.error!);
        }
    }
};

const updateItem = async () => {
    const itemId = parseInt(idInput.value);

    const newName = nameInput.value.trim();
    if (!newName) {
        showError("Name can't be empty!");
        return;
    }

    const item = {
        name: newName,
        isComplete: isCompleteInput.checked,
    };

    const result = await TodoService.update(itemId, item);
    if (result.success) {
        closeError();
        changesMessage.style.display = "none";
        showSuccessMessage();
        await refreshList();
    } else {
        showError(result.error!);
    }
};

const showSuccessMessage = () => {
    successMessage.style.display = "block";
};

const closeEditForm = () => {
    editForm.style.display = "none";
    changesMessage.style.display = "none";
    successMessage.style.display = "none";
};

const displayEditForm = (id: number) => {
    const item = todos.find((item) => item.id === id);
    if (!item) {
        return;
    }

    idInput.value = item.id.toString();
    isCompleteInput.checked = item.isComplete;
    nameInput.value = item.name;
    successMessage.style.display = "none";
    editForm.style.display = "block";
};

const checkForChanges = () => {
    const id = parseInt(idInput.value);
    const item = todos.find((t) => t.id === id);

    if (!item) return;

    const nameChanged = nameInput.value !== item.name;
    const statusChanged = isCompleteInput.checked !== item.isComplete;

    if (nameChanged || statusChanged) {
        changesMessage.style.display = "block";
        successMessage.style.display = "none";
    } else {
        changesMessage.style.display = "none";
    }
};

const displayCount = (itemCount: number) => {
    const name = itemCount === 1 ? "item" : "items";
    const counter = document.getElementById("counter");
    if (!counter) {
        return;
    }

    counter.innerText = `Total: ${itemCount} ${name}`;
};

const renderTable = async () => {
    tableBody.innerHTML = "";

    displayCount(todos.length);

    todos.forEach((item) => {
        let isCompleteCheckbox = document.createElement("input");
        isCompleteCheckbox.type = "checkbox";
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.className = "form-check-input";
        isCompleteCheckbox.checked = item.isComplete;

        let editButton = document.createElement("button");
        editButton.innerText = "Edit";
        editButton.className = "btn btn-warning me-2";
        editButton.addEventListener("click", () => displayEditForm(item.id));

        let deleteButton = document.createElement("button");
        deleteButton.innerText = "Delete";
        deleteButton.className = "btn btn-danger";
        deleteButton.addEventListener("click", () => deleteItem(item.id));

        let tr = tableBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.className = "align-middle";
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.className = "text-end";
        td3.appendChild(editButton);
        td3.appendChild(deleteButton);
    });
};

init();
