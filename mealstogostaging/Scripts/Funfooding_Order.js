jQuery(document).ready(function () {
    //Add to cart


    jQuery('#PickUpTime #LastOrderTime').datepicker({ dateFormat: "dd/mm/yy HH:MM tt" });

    //remove from cart
    jQuery('#remove').click(function () {
        var id = 1;
        jQuery.ajax("Home/RemoveealFromCart",
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
    jQuery('#remove').click(function () {
        var id = 1, qty = 1;
        jQuery.ajax("Home/UpdatetoCart",
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
    if (jQuery('#profilepic').length > 0) {
        jQuery('#Photo').hide();
    }
    else {
        jQuery('#Photo').show();
    }
    jQuery('.RemoveProfilePhoto').click(function () {
        var id = jQuery(this).attr('name');
        jQuery.ajax("/User/RemovePhoto",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 jQuery('#Photo').show();
                 jQuery('.RemoveProfilePhoto').hide();
                 jQuery('#profilepic').hide();

                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });

    jQuery('.RemoveMealPhoto').click(function () {
        var id = jQuery(this).attr('name');
        jQuery.ajax("/MealItem/RemovePhoto",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 jQuery('#Photo').show();
                // jQuery('.RemoveMealPhoto').hide();
                 jQuery('#' + id).hide();

                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });

    jQuery('.DeleteMealItem').click(function () {
        var id = jQuery(this).attr('name');
        if (confirm('Are you sure you want to delete this record?')) {
            jQuery.ajax("/MealItem/Delete",
             {
                 data: { id: id },
                 type: "POST",
                 success: function (data) {
                     location.reload(true);
                     //jQuery('#' + id).remove();
                     //if (jQuery('#' + id).length == 0) {
                     //    jQuery('#photos').hide();
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
    $(document).on('click', '.delete', function() {
        if (confirm('Are you sure?')) {
            $.ajax({
                url: this.href,
                type: 'POST',
                context: this,
                success: function(result) {
                    $(this).closest('div').remove();
                }
            });
        }
        return false;
    })


    jQuery('.RemovePhoto').click(function () {
        var id = jQuery(this).attr('name');
        jQuery.ajax("/MealItem/RemovePhoto",
         {
             data: { id: id },
             type: "POST",
             success: function (data) {
                 jQuery('#' + id).remove();
                 if (jQuery('#' + id).length == 0) {
                     jQuery('#photos').hide();
                 }
                 //Remove and Display no. records in cart at the header section
             },
             error: function (e, s, t) {
                 alert(e);
             }
         });
    });


    $('body').on('focus', ".datepicker_recurring_start", function () {
        $(this).datetimepicker();
    });

    jQuery('#aMealAdSchedules').click(function () {
        var nextid = jQuery('#tblMealAdSchedules').children().children().length;

        //var newRow = '<tr><td><input class="text-box single-line" data-val="true" data-val-date="The field PickUpStartDateTime must be a date." data-val-required="The PickUpStartDateTime field is required." id="MealAdSchedules_' + nextid + '__PickUpStartDateTime" name="MealAdSchedules[' + nextid + '].PickUpStartDateTime" type="datetime" value="">' +
        //'<span class="field-validation-valid" data-valmsg-for="MealAdSchedules[' + nextid + '].PickUpStartDateTime" data-valmsg-replace="true"></span>&nbsp;' +
        //'<input class="text-box single-line" data-val="true" data-val-date="The field PickUpEndDateTime must be a date." data-val-required="The PickUpEndDateTime field is required." id="MealAdSchedules_' + nextid + '__PickUpEndDateTime" name="MealAdSchedules[' + nextid + '].PickUpEndDateTime" type="datetime" value="">' +
        //'<span class="field-validation-valid" data-valmsg-for="MealAdSchedules[' + nextid + '].PickUpEndDateTime" data-valmsg-replace="true"></span></td></tr>';
        //   var newRow = '<tr><td>Hello</td></tr>';

        //        var newRow =
        //                '<tr><td>' +
        //                   ' <link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet" type="text/css" />' +

        //'<link href="http://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/css/datepicker.css" rel="stylesheet" type="text/css" />' +
        //'<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>' +

        //'<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>' +
        //'<script src="http://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/js/bootstrap-datepicker.js"></script>' +
        //'<script src="~/build/js/moment.js"></script>' +
        //           ' <script type="text/javascript">'+
        //    '$(function () {'+
        //     '   $('+'\'#dateF1\''+').datetimepicker({'+
        //       ' });'+
        //    '});'+
        //        '</script>'+
        //        '<script type="text/javascript">'+
        //          '  $(function () {'+
        //               ' $(' + '\'#dateT1\'' + ').datetimepicker({' +
        //              '  });' +


        //            '});' +
        //             '$(' + '\'body\'' + ').on(' + '\'focus\'' + ',".datepicker_recurring_start", function(){' +
        //                 ' $(this).datetimepicker();' +

        //        '</script>'+






        //'<div id="dateF'+ nextid + '" class="input-group date">' +
        //'<input id="MealAdSchedules_' + nextid + '_PickUpStartDateTime" class="text-box single-line" type="datetime" value="" name="MealAdSchedules[0].PickUpStartDateTime" data-val-required="The PickUpStartDateTime field is required." data-val-date="The field PickUpStartDateTime must be a date." data-val="true">'+
        //'<span class="input-group-addon">'+
        //'</div>'+
        //'<span class="field-validation-valid" data-valmsg-replace="true" data-valmsg-for="MealAdSchedules[0].PickUpStartDateTime"></span>'+
        //'<div id="dateT'+ nextid + '" class="input-group date">' +
        //'<input id="MealAdSchedules_' + nextid + '_PickUpEndDateTime" class="text-box single-line" type="datetime" value="" name="MealAdSchedules[0].PickUpEndDateTime" data-val-required="The PickUpEndDateTime field is required." data-val-date="The field PickUpEndDateTime must be a date." data-val="true">'+
        //'<span class="input-group-addon">'+
        //'<span class="glyphicon glyphicon-calendar"></span>'+
        //'</span>'+
        //'</div>'+
        //'<span class="field-validation-valid" data-valmsg-replace="true" data-valmsg-for="MealAdSchedules[0].PickUpEndDateTime"></span>'+
        //'<script src="/Scripts/Funfooding_Order.js" type="text/javascript">'+
        //'<script src="/build/js/bootstrap-datetimepicker.min.js">'+'<script src="/build/js/bootstrap-datetimepicker1.js">'+
        //        '<p> </p>' +
        //       ' <script type="text/javascript" src="~/Scripts/Funfooding_Order.js"></script>'+
        //'<script src="~/build/js/bootstrap-datetimepicker.min.js"></script>'+
        //'<script src="~/build/js/bootstrap-datetimepicker1.js"></script>'

        //'</td>'+
        //'</tr>' 
        //$('#content').append('<br>a datepicker <input class="datepicker_recurring_start"/>');
        //$('body').on('focus', ".datepicker_recurring_start", function () {
        //    $(this).datepicker();
    });

    //jQuery('#tblMealAdSchedules tr:last').after(newRow);

    //        return false;
    //    });

    //    $("#aMealAdSchedules").on("focusin", ".dependent-datepciker", function () {
    //        $(this).datetimepicker();
    //    });
});
$('#cmd').click(function () {

    cnt = $('#content input').size();

    if (cnt == 3) {
        cnt--;
    }

    nextid = cnt;

    $('#content').append('<br> <div style="float:left;"><div style="float:left;" class="input-group date" id="datetimepicker3"><input  required id=\'MealAdSchedules_' + nextid + '__PickUpStartDateTime\' name=\'MealAdSchedules[' + nextid + '].PickUpStartDateTime\' class="datepicker_recurring_start" data-val-required="The PickUpStartDateTime field is required." value="09/30/2015 4:03 PM"/> <span class="input-group-addon" onclick="put_focus_start(' + nextid + ')"> <span class="glyphicon glyphicon-calendar"></span></span></div>' + '<div style="float:left; margin:0 1em 2em;"> To </div> ' + '<div id="datetimepicker4" class="input-group date" style="float:left;"><input required  id=\'MealAdSchedules_' + nextid + '__PickUpEndDateTime\' name=\'MealAdSchedules[' + nextid + '].PickUpEndDateTime\' class="datepicker_recurring_start data-val-required="The PickUpStartDateTime field is required." value="09/30/2015 4:03 PM"/><span class="input-group-addon" onclick="put_focus_end(' + nextid + ')"> <span class="glyphicon glyphicon-calendar"></span></span></div></div>');
    // $('#content').append('<script type="text/javascript">$(function () {$(\'MealAdSchedules_' + nextid + '__PickUpStartDateTime\').datetimepicker({});});</script><script type="text/javascript">$(function () {$(\'MealAdSchedules_' + nextid + '__PickUpEndDateTime\').datetimepicker({});});</script><div id="datetimepicker1" class="input-group date" >@*<input  type="text" name="From" class="form-control">*@ @Html.EditorFor(model => model.PickUpStartDateTime, new { @class = "gen-ftb", required = "required" })<span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span> </span> </div>');

});

function put_focus_start(id)
{
    $('#MealAdSchedules_'+id+'__PickUpStartDateTime').focus();

}

function put_focus_end(id) {
    $('#MealAdSchedules_' + id + '__PickUpEndDateTime').focus();

}