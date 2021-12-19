//определяем мобильное устройство
window.isMobile = () => {
    if (window) {
        return window.matchMedia(`(max-width: 767px)`).matches
    }
    return false
}