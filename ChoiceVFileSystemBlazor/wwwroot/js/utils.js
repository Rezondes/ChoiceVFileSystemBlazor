function setFavicon(iconUrl) {
    let link = document.querySelector("link[rel~='icon']");
    if (!link) {
        link = document.createElement('link');
        link.rel = 'icon';
        document.getElementsByTagName('head')[0].appendChild(link);
    }
    link.href = iconUrl;
}

function downloadFileFromUrl(filename, url) {
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    link.click();
}