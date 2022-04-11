
let dropArea = document.getElementById('drop-area');

dropArea.addEventListener('dragenter', handlerFunction, false);
dropArea.addEventListener('dragleave', handlerFunction, false);
dropArea.addEventListener('dragover', handlerFunction, false);
dropArea.addEventListener('drop', handlerFunction, false);


let workProjects = null;

;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    dropArea.addEventListener(eventName, preventDefaults, false)
})

function preventDefaults(e) {
    e.preventDefault()
    e.stopPropagation()
}

;['dragenter', 'dragover'].forEach(eventName => {
    dropArea.addEventListener(eventName, highlight, false)
})

    ;['dragleave', 'drop'].forEach(eventName => {
        dropArea.addEventListener(eventName, unhighlight, false)
    })

function highlight(e) {
    dropArea.classList.add('highlight')
}

function unhighlight(e) {
    dropArea.classList.remove('highlight')
}

dropArea.addEventListener('drop', handleDrop, false)

async function handleDrop(e) {
    let dt = e.dataTransfer
    let files = dt.files

    await handleFiles(files)
}

async function handleFiles(file) {
    await uploadFile(file)
}

async function uploadFile(file) {
    debugger;
    let url = 'https://localhost:7226/csv/post'

    let data = new FormData();
    data.append("file", file[0]);

    const response = await fetch(url, {
        method: 'POST',
        body: data
    });


    let data2;
    let sharedWork;
    response.json().then(data => {

        var newData = new FormData();
        workProjects = data;
        let stringData = JSON.stringify(data);
        newData.append("sharedWork", stringData);

        var result = await fetch("https://localhost:7070/csv/index", {
            method: 'POST',
            body: newData
        });

        var stringResult = JSON.stringify(result);
    });


}


