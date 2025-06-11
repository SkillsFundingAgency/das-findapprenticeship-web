async function autosave(el, config) {
    const formData = new FormData(el)
    config?.beforeSubmit(formData)
    const token = el.querySelector('input[name="__RequestVerificationToken"]')?.value
    try {
        await fetch(config.url, {
            method: config.method,
            headers: { "RequestVerificationToken": token },
            body: formData
        })
    } catch (error) {
        console.error("dasAutosave: failed to save form", error)
    }
}

function getConfig(options) {
    const defaults = {
        intervalInSeconds: 300,
        method: "POST",
        url: "",
    }

    const config = Object.assign({}, defaults, options)
    config.intervalInSeconds = Number(config.intervalInSeconds)
    config.isValid = () => !Number.isNaN(config.intervalInSeconds)
        && config.intervalInSeconds > 0
        && config.url?.length > 0
        && config.method?.length > 0
    return config
}

export default function (formEl, options) {
    if (!formEl) {
        console.error("dasAutosave: the form element is a required parameter")
        return;
    }
    
    const config = getConfig(options)
    if (!config.isValid()) {
        console.error(`dasAutosave: options are incomplete`)
        return
    }

    const autosaveTimer = setInterval(async () => await autosave(formEl, config), config.intervalInSeconds * 1000)
    formEl.addEventListener("submit", (e) => clearInterval(autosaveTimer))
}