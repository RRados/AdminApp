// Example starter JavaScript for disabling form submissions if there are invalid fields
(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var element = document.getElementById('validationCustom02');
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        // eslint-disable-next-line no-unused-vars
        var validation = Array.prototype.filter.call(forms, function (form) {
            forms.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                forms.classList.add('was-validated');
                element.classList.add('was-validated');
            }, false);
        });
    }, false);
})();