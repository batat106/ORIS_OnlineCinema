const form = document.querySelector("#userinfo");

document.addEventListener("DOMContentLoaded", async function (){
    setTimeout(async () =>{
        ajaxGet().then(res => {});
    } , 1500);  
})

function ajax_method(element) {
    const formdata = new FormData(form);
    let xhr = new XMLHttpRequest();
    xhr.open("POST", element.getAttribute("action"));
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200 && this.status === 302) {
            document.getElementById("submit_message").innerHTML = this.responseText;
        }
    };
}

function ajaxGet() {
    return new Promise((resolve, reject) => {
        let xhr = new XMLHttpRequest();
        xhr.open("GET", "templator");
        xhr.send();
        xhr.onreadystatechange = function () {
            if (this.readyState === 4 && this.status === 200) {
                document.getElementById("catalog").insertAdjacentHTML('afterend', this.responseText);
                return resolve(this.responseText);
            }
        };
    })
}

form.addEventListener("submit", (event) => {
    event.preventDefault();
    sendData();
});
