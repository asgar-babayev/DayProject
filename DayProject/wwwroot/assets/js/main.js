

let btns = document.querySelectorAll(".btncat");
let imgs = document.querySelectorAll(".imgs");
let btnall = document.querySelector(".btnall");

btnall.addEventListener("click", function () {
    imgs.forEach(y => {
        y.classList.remove("d-none");
    })
})

btns.forEach(x => {
    x.addEventListener("click", function () {
        let id = this.getAttribute("data-id");
        imgs.forEach(y => {
            let imgId = y.getAttribute("data-target");
            if (id == imgId) {
                y.classList.remove("d-none");
            }
            else
                y.classList.add("d-none");
        })
    })
})

imgs.forEach(y => {
    y.style.display = "block";
})

function img() {
    document.querySelectorAll(".link").forEach(x => {
        x.classList.remove("d-none")
    })
}