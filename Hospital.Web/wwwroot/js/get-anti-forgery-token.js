function getAntiForgeryToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]')?.value ||
           document.querySelector('[name="__RequestVerificationToken"]')?.value;
}