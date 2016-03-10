$(document).ready(function () {
    //Add to cart
    $('#PickUpTime #LastOrderTime').datepicker({ dateFormat: "dd/mm/yy HH:MM tt" });
    
    //remove from cart
    $('#remove').click(function () {
        var id = 1;
        $.ajax("Home/RemoveealFromCart",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });
    //Update no. of quontity
    $('#remove').click(function () {
        var id = 1, qty = 1;
        $.ajax("Home/UpdatetoCart",
         {
             data: { id: id, qty: qty },
             type: "POST",
             success: function (data) {
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });
    if ($('#profilepic').length > 0) {
        $('#Photo').hide();
    }
    else {
        $('#Photo').show();
    }
    $('.RemoveProfilePhoto').click(function () {
        var id = $(this).attr('name');
        $.ajax("/User/RemovePhoto",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 $('#Photo').show();
                 $('.RemoveProfilePhoto').hide();
                 $('#profilepic').hide();
                 
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });

    $('.DeleteMealItem').click(function () {
        var id = $(this).attr('name');
        if (confirm('Are you sure you want to delete this record?')) {
            $.ajax("/MealItem/Delete",
             {
                 data: { id: id },
                 type: "POST",
                 success: function (data) {
                     location.reload(true);
                     //$('#' + id).remove();
                     //if ($('#' + id).length == 0) {
                     //    $('#photos').hide();
                     //}
                     //Remove and Display no. records in cart at the header section
                 },
                 error: function (e, s, t) {
                     alert(e);
                 }
             });
        }
    });
    ///

    $('.RemovePhoto').click(function () {
        var id = $(this).attr('name');
        $.ajax("/MealItem/RemovePhoto",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 $('#' + id).remove();
                 if ($('#' + id).length == 0) {
                     $('#photos').hide();
                 }
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });
});