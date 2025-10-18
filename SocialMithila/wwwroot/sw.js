self.addEventListener("push", event => {
    if (!event.data) return;
    const data = event.data.json();
    const options = {
        body: data.body,
        icon: "/images/app-logo.png"
    };
    event.waitUntil(self.registration.showNotification(data.title, options));
});
