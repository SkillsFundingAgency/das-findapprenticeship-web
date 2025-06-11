function dasAutosaveForm(formEl, options) {
    if (!formEl) {
        console.error("dasAutosave: the form element is a required parameter")
        return;
    }

    const saveInterval = Number(options?.intervalInSeconds)
    const saveUrl = options?.url
    if (Number.isNaN(saveInterval) || (saveUrl?.length ?? 0) === 0) {
        console.error(`dasAutosave: options are incomplete`)
        return
    }

    const autosaveTimer = setInterval(async () => await autosave(formEl, saveUrl), saveInterval * 1000)
    formEl.addEventListener("submit", (e) => clearInterval(autosaveTimer))

    async function autosave(el, saveUrl) {
        const formData = new FormData(el)
        options?.beforeSubmit(formData)
        const token = el.querySelector('input[name="__RequestVerificationToken"]')?.value
        try {
            await fetch(saveUrl, {
                method: "POST",
                headers: { "RequestVerificationToken": token },
                body: formData
            })
        } catch {}
    }
}