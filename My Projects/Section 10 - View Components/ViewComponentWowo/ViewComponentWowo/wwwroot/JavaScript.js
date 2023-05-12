let btn = document.querySelector("#load-friends-btn")
let listDiv = document.querySelector("#list")


btn.addEventListener("click",
    async function () {
        let response = await fetch("friends-list", { method: "GET" });
        let responseBody = await response.text();
        console.log(response, responseBody)
        listDiv.innerHTML = responseBody;
    });