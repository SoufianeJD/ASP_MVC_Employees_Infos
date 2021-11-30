$("#printPDF").click(function () {
    var element = document.getElementById('parentDiv');
    html2pdf().from(element).set({
        margin: [30, 10, 5, 10],
        pagebreak: { avoid: 'tr' },
        jsPDF: { orientation: 'landscape', unit: 'pt', format: 'letter', compressPDF: true }
    }).save()
});


//Angularjs and jquery.datatable with ui.bootstrap and ui.utils

var app = angular.module('formvalid', ['ui.bootstrap', 'ui.utils']);
app.controller('validationCtrl', function ($scope) {
   

    $scope.dataTableOpt = {
        //custom datatable options 
        // or load data through ajax call also
        "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
    };
});


if (!Modernizr.touch) {
    // Avoid jump when triggering skip links
    $('.links-select-label, .links-select-skip').on('click keyup', function (e) {
        e.preventDefault();
        if (e.type === 'keyup' && e.which !== 32)	// Space bar
            return;
        $($(this).attr('href')).focus();
    });
}
else {
    // Listen to touchstart, so, touching the body will close the menu.
    $('body').on('touchstart', function (e) { })
        .find('.links-select-label').on('click', function (e) {
            e.preventDefault();
        });
}