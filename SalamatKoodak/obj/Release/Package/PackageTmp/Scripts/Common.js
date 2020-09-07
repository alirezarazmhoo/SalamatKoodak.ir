
function checkvalidity(target) {
    var ErrorText = '';
    var state = 1;
    $('form#' + target + '').find('input').each(function () {
        if ($(this).prop('required') && $(this).val() === "") {
            ErrorText += $(this).attr('display') + " ، ";
            state = 0;  
        }  
    });
    if (state === 0) {
        ErrorText += 'خالی است';
        $("#textError").text(ErrorText);
        $("#ErrorModal").modal('show');
        return 0;
    }
    else {
        return 1;
    }
}
function clear(target) {
    $('form#' + target + '').find('input').each(function () {
        $(this).val('');
    });
}
function CreateModal(target, mode) {
    var modal = "";
    if (parseInt(mode) === 1) {
        modal = "<div class='modal fade' id='ErrorModal' role='dialog'><div class='modal-dialog modal-sm'><div class='modal-content'><div class='modal-header'><h4 class='modal-title text-danger'>خطا</h4></div><div class='modal-body text-danger'><p id='textError'></p></div><div class='modal-footer'><button type='button' class='btn btn-danger'  data-dismiss='modal'>بستن</button>";
        document.getElementById(target).innerHTML = modal;
    }
    else if (parseInt(mode) === 2) {
        modal = "<div class='modal fade' id='SuccessModal' role='dialog'><div class='modal-dialog modal-sm'><div class='modal-content'><div class='modal-header'><h4 class='modal-title text-success'>موفقیت آمیز</h4></div><div class='modal-body text-success'><p>عملیات با موفیت انجام شد</p></div><div class='modal-footer'><button type='button' class='btn btn-success'  data-dismiss='modal'>تایید</button>";
        document.getElementById(target).innerHTML = modal;
    }
    else {
        modal = "<div class='modal fade' id='QuestionModal' role='dialog'><div class='modal-dialog modal-sm'><div class='modal-content'><div class='modal-header'><h4 class='modal-title'>پرسش</h4></div><div class='modal-body text-warning'><p>آیا مایل به انجام عملیات هستید ؟</p></div><div class='modal-footer'><button type='button' class='btn btn-success' onclick='Remove();' data-dismiss='modal'>تایید</button><button type='button' class='btn btn-danger' data-dismiss='modal'>انصراف</button>";
        document.getElementById(target).innerHTML = modal;
    }
}
function PostAjax(ActionName, Parameters,redirecturl) {
    var fd = new FormData();
    for (var i = 0; i < Parameters.length; i++) { 
        if (Parameters[i].special === 'combo') {
            fd.append(Parameters[i].id, $('#' + Parameters[i].htmlname + '').find('option:selected').val());
        }
        else if (Parameters[i].special === 'radio') {
            fd.append(Parameters[i].id, $('input[name="' + Parameters[i].htmlname + '"]:checked').val());
        }
        else {
        fd.append(Parameters[i].id, $('#' + Parameters[i].htmlname + '').val());
        }
    }
    $.ajax({
        type: "POST",
        //url: "@Url.Action('"+ ActionName+"')",
        url: ""+ ActionName+"",
        data: fd,
        dataType: "json",
        contentType: false,
        processData: false,
        beforeSend: function () {
            $("#LoadingModal").modal('show');
        },
        success: function (response) {
            if (response.success) {
                $("#SuccessModal").modal('show');
                clear('register');
                setTimeout(function () { location.href = redirecturl; }, 2000);
            }
            else {
                $("#textError").text(response.responseText);
                $("#ErrorModal").modal('show');
            }
        },
        error: function (response) {
            $("#LoadingModal").modal('show');

        },
            complete: function () {
            $("#LoadingModal").modal('toggle');
        }
    });
}
function CreateAllModals() {
    CreateModal('Error', 1);
    CreateModal('Success', 2);
    CreateModal('Question', 3);
}
function FillComboBox(ActionName,Target) {
    $.ajax({
        type: "Get",
        url: "" + ActionName + "",
        dataType: "json",
        contentType: false,
        processData: false,

        success: function (response) {
            if (response.success) {                  
                $.each(response.list, function () {
               $('#' + Target + '').append($("<option     />").val(this.Id).text(this.Name));
                });
            }
            else {
                $("#textError").text(response.responseText);
                $("#ErrorModal").modal('show');
            }
        },
        error: function (response) {
            $("#LoadingModal").modal('show');

        }
    });
}
function EditAjax(ActionName, id) {
 
    var fd = new FormData();
    fd.append('UserId', id);
    $.ajax({
        type: "Post",
        url: "" + ActionName + "",
        data: fd,
        dataType: "json",
        contentType: false,
        processData: false,
        beforeSend: function () {
            $("#LoadingModal").modal('show');
        },
        success: function (response) {
  

            if (response.success) {
                $.each(response.listItem, function () {
                    $('#' + this.Key + '').val(this.Value);
                });
                $("#CityId option").each(function () {
                    if (this.value == response.cityid) {
                        
                        $(this).prop('selected', true);
                    }
                });
                $('#myModal').modal('show');
            }
            else {
                $("#textError").text(response.responseText);
                $("#ErrorModal").modal('show');
            }
        },
        error: function (response) {
            $("#LoadingModal").modal('show');

        },
        complete: function () {
           

            $("#LoadingModal").modal('toggle');
        }
    });


}
function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        textbox.addEventListener(event, function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    });
}

function SetInputFilter(targets) {
    for (var i = 0; i < targets.length; i++) {
        setInputFilter(document.getElementById(targets[i]), function (value) {
            return /^\d*\.?\d*$/.test(value);
        });
    }
}
function ResetListBox(targets) {

    for (var i = 0; i < targets.length; i++) {
        $('#' + targets[i] + '').prop('selectedIndex', 0);
        }

}

function PersonelEditAjax(ActionName, id) {
    var fd = new FormData();
    fd.append('PersonelId', id);
    $.ajax({
        type: "Post",
        url: "" + ActionName + "",
        data: fd,
        dataType: "json",
        contentType: false,
        processData: false,
        beforeSend: function () {
            $("#LoadingModal").modal('show');
        },
        success: function (response) {
            if (response.success) {
                $.each(response.listItem, function () {
                    $('#' + this.Key + '').val(this.Value);
                });
                $("#RelationTypeId option").each(function () {
                    if (this.value == response.relationtypeid) {
                        $(this).prop('selected', true);
                    }
                });
                $("#PersonelStatusId option").each(function () {
                    if (this.value == response.statusid) {
                        $(this).prop('selected', true);
                    }
                });
                $("#NetWorkType option").each(function () {
                    if (this.value == response.networktype) {
                        
                        $(this).prop('selected', true);
                    }
                });
                $('#myModal').modal('show');
            }
            else {
                $("#textError").text(response.responseText);
                $("#ErrorModal").modal('show');
            }
        },
        error: function (response) {
            $("#LoadingModal").modal('show');

        },
        complete: function () {
            $("#LoadingModal").modal('toggle');
        }
    });


}