const route = (event) => {
    event = event || window.event;
    event.preventDefault();
    window.history.pushState({}, "", event.target.href);
    handleLocation();
};
const routes = {
    404: "/pages/404.html",
    "/": "/pages/index.html", 
    "/about": "/pages/about.html",
    "/contact": "/pages/contact.html"
};

window.onpopstate = handlelocation;
window.route = route;

handleLocation();