var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++)
{
    acc[i].onclick = function ()
    {
        this.classList.toggle("active");
        var panel = this.nextElementSibling;
        if (panel.style.maxHeight) {
            panel.style.maxHeight = null;
        } else {
            panel.style.maxHeight = panel.scrollHeight + "px";
        }
    }
}

window.onscroll = function () {
    scrollFunction();

};

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("myBtn").style.display = "block";
    } else {
        document.getElementById("myBtn").style.display = "none";
    }

}

function topFunction() {

    document.body.scrollTop = 0;

    document.documentElement.scrollTop = 0;

}

