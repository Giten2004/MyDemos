jQuery.validator.addMethod("agerange", function (value, element, params) {
    value = value.replace(/(^\s*)(\s*$)/g, "");
    if (!value) {
        return true;
    }
    var minAge = params.minage;
    var maxAge = params.maxage;

    var birthDateArray = value.split("-");
    var birthDate = new Date(birthDateArray[0], birthDateArray[1],
         birthDateArray[2]);
    var currentDate = new Date();
    var age = currentDate.getFullYear() - birthDate.getFullYear();
    return age >= minAge && age <= maxAge;
});

jQuery.validator.unobtrusive.adapters.add("agerange", ["minage", "maxage"],
    function (options) {
        options.rules["agerange"] = {
            minage: options.params.minage,
            maxage: options.params.maxage
        };
        options.messages["agerange"] = options.message;
    });
