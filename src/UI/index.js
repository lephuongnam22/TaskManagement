const serverUrl = "http://localhost:5200/tasks";

$(document).ready(function() {
    $('#addTask').on('hidden.bs.modal', function(e)
    { 
        var element = document.getElementById("addTaskForm");
        element.reset();
    });

    $('#addTask').on('shown.bs.modal', function(event) {
        loadTaskStatusDropDown("dialog-task-status-select");
        loadPriorityDropDown("dialog-task-priority-select");

        var button = $(event.relatedTarget);
        var id = button.data('id');

        // Edit
        if(id) {
            loadDialogData(id);
        }

        var element = document.getElementById('dialog-add-task-title');
        element.innerText = "Edit Task";

        let addTask = document.getElementById("addTaskForm");

        addTask.addEventListener("submit", (e) => {
            showLoader();
            onAddDialogSubmit(e);
        });
    });

    $('#deleteTask').on('shown.bs.modal', function(event) {
        var deleteTaskForm = document.getElementById('deleteTaskForm');

        var button = $(event.relatedTarget);
        var id = button.data('id');
        var title = button.data('title');

        if(id > 0) {
            selectElement('dialog-delete-task-id', id);
            document.getElementById('dialog-delete-task-title').innerHTML = title;

            if(deleteTaskForm) {
                deleteTaskForm.addEventListener("submit", (e) => {
                    $('#deleteTask').modal('hide');
                    showLoader();
                    onDeleteDialogSubmit(e);
                    e.preventDefault();
                });
            }
        }
    });
});

window.addEventListener("load", (event) => {
    showLoader();
    onLoad();
    searchTask();
});

function searchTask() {
    let searchTask = document.getElementById("search-task");

    if(searchTask) {
       
        searchTask.addEventListener("submit", (e) => {
            showLoader();
            e.preventDefault();
            const formData = new FormData(e.target);

            var searchTaskModel = {
                taskStatus: formData.get('search-task-status'),
                priority: formData.get('search-task-priority'),
            };

            var jsonData = JSON.stringify(searchTaskModel);

            $.ajax({
                type: "POST",
                url: serverUrl + "/search",
                dataType: "json",
                contentType: "application/json",
                processData: false,
                success: function (data) {
                    if (data) {
                        clearTable();
                        setDataToTable(data);
                        $('#addTask').modal('hide');
                        hideLoader();
                    }
                },
                data: jsonData,
                error: function(e) {
                    hideLoader();
                    showErrorDialog("Search Task Error");
                }
            });
        });
    }

    
}

function onLoad() {
    
    loadTaskStatusDropDown("task-status-select");
    loadPriorityDropDown("task-priority-select");

    $.ajax({
        type: "GET",
        url: serverUrl,
        dataType: 'json',
        dataSrc: '',
        crossDomain: true,
        'ccess-Control-Allow-Origin': "*",
        success: function(data) {
            setDataToTable(data);
            hideLoader();
        },
        failure: function(e) {
            hideLoader();
        }
    });
}

function setTaskToTable(element) {
    let table = document.getElementById("tableBody");
    let row = document.createElement("tr");

    let c0 = document.createElement("td");
    c0.style.display = "none";
    c0.innerText = element.id;

    let c1 = document.createElement("td");
    c1.innerText = element.title;

    let c2 = document.createElement("td");
    c2.innerText = element.description;

    let c3 = document.createElement("td");
    c3.innerText = element.taskStatus;

    let c4 = document.createElement("td");
    c4.innerText = element.dueDate;

    let c5 = document.createElement("td");
    c5.innerText = element.priority;

    let c6 = document.createElement("td");

    c6.innerHTML = '<button class="btn btn-primary btn-action mr-1" data-toggle="modal" data-target="#addTask"' +
     ' data-id="'+element.id+'"' +
    'data-original-title="Edit"><i class="fas fa-pencil-alt"></i></button>' +
    '<button class="btn btn-danger btn-action" data-toggle="modal" data-target="#deleteTask"' +
    ' data-id="'+element.id+'" data-title="' + element.title + '" ' +
    'data-original-title="Delete"><i class="fas fa-trash"></i></button>';

    row.appendChild(c0);
    row.appendChild(c1);
    row.appendChild(c2);
    row.appendChild(c3);
    row.appendChild(c4);
    row.appendChild(c5);
    row.appendChild(c6);

    table.appendChild(row);
}

function onAddDialogSubmit(e) {
    e.preventDefault();

    const formData = new FormData(e.target);

    var taskModel = {
        id: 0,
        title: formData.get('title'),
        description: formData.get('description'),
        taskStatus: formData.get('taskStatus'),
        priority: formData.get('taskPriority'),
        dueDate: formData.get('dueDate')
    };

    // Edit
    var id = formData.get('task-id');
                
    if(id > 0) {
        taskModel.id = id;
        onEditTask(taskModel);
    }
    else {
        onAddTask(taskModel);
    }
}

function onEditTask(taskModel) {
    var jsonData = JSON.stringify(taskModel);

    $.ajax({
        type: "PUT",
        url: serverUrl,
        dataType: "json",
        contentType: "application/json",
        processData: false,
        success: function (task) {
            if (task) {
                $('#addTask').modal('hide');
                setRowData(task);
                hideLoader();
            }
        },
        data: jsonData,
        error: function(e) {
            hideLoader();
            showErrorDialog("Edit Task Error");
            e.preventDefault();
        }
    });
}

function onAddTask(taskModel) {
    var jsonData = JSON.stringify(taskModel);

    $.ajax({
        type: "POST",
        url: serverUrl,
        dataType: "json",
        contentType: "application/json",
        processData: false,
        success: function (task) {
            if (task) {
                setTaskToTable(task);
                $('#addTask').modal('hide');
                hideLoader();
            }
        },
        data: jsonData,
        error: function(e) {
            hideLoader();
            showErrorDialog("Add Task Error");
            e.preventDefault();
        }
    });
}

function setDataToTable(data) {

    if(data) {
        data.forEach(element => {
            setTaskToTable(element);
        });
    }
}

function loadTaskStatusDropDown(elementName) {
    $.ajax({
        type: "GET",
        url: serverUrl + "/task-status",
        dataType: 'json',
        dataSrc: '',
        crossDomain: true,
        'ccess-Control-Allow-Origin': "*",
        success: function(data) {
            removeOptions(elementName);
            setTaskDropdown(data, elementName);
        },
        error: function(request, status, error) {
            hideLoader();
            showErrorDialog("Load Data Error");
        }
    });
}

function setTaskDropdown(data, elementName) {
    let select = document.getElementById(elementName);

    data.forEach(element => {
        const opt = document.createElement("option");
        opt.value = element;
        opt.text = element;
        select.options.add(opt);
    });
}

function loadPriorityDropDown(elementName) {
    $.ajax({
        type: "GET",
        url: serverUrl + "/task-priority",
        dataType: 'json',
        dataSrc: '',
        crossDomain: true,
        'ccess-Control-Allow-Origin': "*",
        success: function(data) {
            removeOptions(elementName);
            setTaskDropdown(data, elementName);
        },
        error: function() {
            hideLoader();
            showErrorDialog("Load Data Error");
        }
    });
}


function selectElement(id, valueToSelect) {    
    let element = document.getElementById(id);

    if(element) {
        element.setAttribute('value', valueToSelect);
        element.value = valueToSelect;
    }
}

function editTask(id) {
    console.log(id);

    $('#addTask').modal('show');
}

function loadDialogData(id) {

    $.ajax({
        type: "GET",
        url: serverUrl + "/" + id,
        dataType: 'json',
        dataSrc: '',
        crossDomain: true,
        'ccess-Control-Allow-Origin': "*",
        success: function(data) {
            if(data) {
                var title = document.getElementById("dialog-task-title");
                
                if(title) {
                    title.value = data.title;
                }

                selectElement('dialog-task-title', data.title);
                selectElement('dialog-task-description', data.description);
                selectElement('dialog-task-status-select', data.taskStatus);
                selectElement('dialog-task-priority-select', data.priority);
                selectElement('dialog-add-task-id', data.id);
                setDueDate(data.dueDate);
            }
        }
    });
}

function setDueDate(dueDate) {
    var date = new Date(dueDate);
    const formatDate = date.getDate() < 10 ? `0${date.getDate()}`:date.getDate();
    var month = date.getMonth() + 1;
    const formatMonth = month < 10 ? `0${month}`: month;

    let element = document.getElementById('dialog-task-dueDate');
    element.value = date.getFullYear() + "-" + formatMonth + "-" + formatDate;
}

function setRowData(task) {
    var table = document.getElementById('tableBody');

    for (var r = 0; r < table.rows.length; r++) {

        if(table.rows[r].cells[0].innerText == task.id) {
            table.rows[r].cells[1].innerText = task.title;
            table.rows[r].cells[2].innerText = task.description;
            table.rows[r].cells[3].innerText = task.taskStatus;
            table.rows[r].cells[4].innerText = task.dueDate;
            table.rows[r].cells[5].innerText = task.priority;
        }
    }
}

function removeRow(id) {
    var table = document.getElementById('tableBody');
    var index = -1;

    for (var r = 0; r < table.rows.length; r++) {

        if(table.rows[r].cells[0].innerHTML == id) {
            index = r;
        }
    }

    if(index != -1) {
        table.deleteRow(index);
    }
    
}

function removeOptions(selectElement) {
    var dropdown = document.getElementById(selectElement);

    while (dropdown.length > 0) {                
        dropdown.remove(0);
    }
 }

 function onDeleteDialogSubmit(e) {
    const formData = new FormData(e.target);

    var id = formData.get('task-id');

    if(id > 0) {
        $.ajax({
            type: "DELETE",
            url: serverUrl + "/" + id,
            dataType: 'json',
            dataSrc: '',
            crossDomain: true,
            'ccess-Control-Allow-Origin': "*",
            success: function(data) {
                if(data) {
                    removeRow(id);
                    $('#deleteTask').modal('hide');
                }
            },
            error: function(e) {
                hideLoader();
                showErrorDialog("Delete Task Error");
                e.preventDefault();
            }
        });
    }

    return false;
}

function showLoader() {
    var element = document.getElementById('loading-state');

    element.style.display = "flex";
}

function hideLoader() {
    var element = document.getElementById('loading-state');

    element.style.display = "none";
}

function showErrorDialog(messageTitle) {

    var element = document.getElementById('dialog-errorModal-title');
    element.innerText = messageTitle;
    $('#errorModal').modal('show');
}

function clearTable() {
    var table = document.getElementById("tableBody");
    
    for(var i = table.rows.length - 1; i >= 0; i--)
    {
        table.deleteRow(i);
    }
}