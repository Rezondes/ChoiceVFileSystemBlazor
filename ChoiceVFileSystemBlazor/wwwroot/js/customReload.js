window.initializeCustomReload = () => {
    document.addEventListener('keydown', function (event) {
        if (event.key === 'F5') {
            event.preventDefault();
            DotNet.invokeMethodAsync('ChoiceVFileSystemBlazor', 'CustomReload');
        }
    });
};