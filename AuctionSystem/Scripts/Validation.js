$(document).ready(function () {
    $('#submitBid').click(function (event) {

        if ($('#submitBidPrice').val() == "") {
            alert('Bid price is required');
            event.preventDefault();
        }
        else if (!($.isNumeric($('#submitBidPrice').val()))) {
            alert('Enter valid Bid Price');
            event.preventDefault();
        }
    });


    $('#updateBid').click(function (event) {

        if ($('#bidPrice').val() == "") {
            alert('Bid price is required');
            event.preventDefault();
        }
        else if (!($.isNumeric($('#bidPrice').val()))) {
            alert('Enter valid Bid Price');
            event.preventDefault();
        }
    });


    $('#updateProduct').click(function (event) {
        var date = Date.parse($('#bidTime').val());
        if ($('#productName').val() == "" || $('#description').val() == "" || $('#bidTime').val() == "") {
            alert('Please enter required fields');
            event.preventDefault();
        }
        else if (!$.isNumeric(date)) {
            alert('Product Bid Time is not valid');
            event.preventDefault();
        }
    });

    $('#submitProduct').click(function (event) {
        var date = Date.parse($('#sellProductBidTime').val());
        if ($('#sellProductName').val() == "" || $('#sellProductDescription').val() == ""
            || $('#sellProductBidTime').val() == "" || $('#sellProductBidPrice').val() == "") {
            alert('Please enter required fields');
            event.preventDefault();
        }
        else if (!$.isNumeric(date)) {
            alert('Product Bid Time is not valid');
            event.preventDefault();
        }
        else if (!($.isNumeric($('#sellProductBidPrice').val()))) {
            alert('Enter valid Bid Price');
            event.preventDefault();
        }
    });

    //$("#sellProductBidTime").datepicker({
    //    dateFormat: "M dd yy",
    //    changeMonth: true,
    //    changeYear: true,
    //    yearRange: "-60:+0"
    //});

    $('#register').click(function (event) {

        if ($('#firstName').val() == "" || $('#lastName').val() == ""
            || $('#phoneNumber').val() == "" || $('#email').val() == "" || $('#password').val() == ""
            || $('#confirmPassword').val() == "" || $('#address').val() == ""
            || $('#city').val() == "" || $('#state').val() == ""
            || $('#zipcode').val() == "" || $('#country').val() == ""
            || $('#cardNumber').val() == "" || $('#pin').val() == ""
            || $('#cardExpiryDate').val() == "" || $('#holderName').val() == "") {
            alert('Please enter required fields');
            event.preventDefault();
        }

    });


    $('#login').click(function (event) {
       
        if ($('#loginEmail').val() == "" || $('#loginPassword').val() == "" ) {
            alert('Please enter required fields');
            event.preventDefault();
        }
        
    });
});