const form = document.querySelector("#userinfo");
function ajax_method(element) {
    const formdata = new FormData(form);
    let xhr = new XMLHttpRequest();
    xhr.open("POST", element.getAttribute("action"));
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200 && this.status === 302) {
            alert("ABOBA")
            document.getElementById("submit_message").innerHTML = this.responseText;
        }
    };

}
form.addEventListener("submit", (event) => {
    event.preventDefault();
    sendData();
});
