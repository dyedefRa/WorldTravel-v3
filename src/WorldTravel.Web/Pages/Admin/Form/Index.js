$(function () {
    var l = abp.localization.getResource('WorldTravel');
    //var createModal = new abp.ModalManager(abp.appPath + 'Admin/Form/Create');
    //var editModal = new abp.ModalManager(abp.appPath + 'Admin/Form/Edit');

    var getFilter = function () {
        return {
            fullNameFilter: $("#FullNameFilter").val(),
            emailFilter: $("#EmailFilter").val(),
            phoneFilter: $("#PhoneFilter").val(),
            isContactedFilter: $("#IsContactedFilter").val()
        };
    };

    var dataTable = $('#FormsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(worldTravel.services.form.getFormList, getFilter),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('ChangeContactedStatus'),
                                    //visible: abp.auth.isGranted('WorldTravel.Forms.Edit'),
                                    action: function (data) {
                                        worldTravel.services.form.updateFormIsContacted(data.record.id)
                                            .done(function (result) {
                                                if (result.success) {
                                                    toastr.success(result.message)
                                                    dataTable.ajax.reload();
                                                }
                                                else {
                                                    toastr.error(result.message)
                                                }
                                            })
                                            .fail(function () {
                                                toastr.error('@L["UnexpectedError"].Value');
                                            });
                                    }
                                }
                            ]
                    }
                },
                {
                    title: l('FullName'),
                    data: "name",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('PhoneNumber'),
                    data: "phoneNumber",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('Email'),
                    data: "email",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('Gender'),
                    data: "gender",
                    render: function (data) {
                        return l('Enum:GenderType:' + data);
                    }
                },
                {
                    title: l('FormRegType'),
                    data: "formRegType",
                    render: function (data) {
                        return data;
                    }
                },
                //{
                //    title: l('BirthDate'),
                //    data: "birthDate",
                //    render: function (data) {
                //        return setDate(data);
                //    }
                //},
                {
                    title: l('CountryName'),
                    data: "countryName",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('IsContacted'),
                    data: "isContacted",
                    render: function (data) {
                        return setBoolean(data);
                    }
                },
                {
                    title: l('Description'),
                    data: "description",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('CreatedDate'),
                    data: "createdDate",
                    render: function (data) {
                        return setDate(data);
                    }
                },
                //{
                //    title: l('Status'),
                //    data: "status",
                //    render: function (data) {
                //        return l('Enum:Status:' + data);
                //    }
                //}
            ],
            createdRow: function (nRow, aData) {
            }
        })
    );


    //createModal.onResult(function () {
    //    dataTable.ajax.reload();
    //    abp.notify.info(l('Successfully'));
    //});

    //editModal.onResult(function () {
    //    dataTable.ajax.reload();
    //    abp.notify.info(l('Successfully'));
    //});

    //$('#createButton').click(function (e) {
    //    e.preventDefault();
    //    createModal.open();
    //});

    $('#btnSearch').on('click', function (e) {
        $('.loading').show();

        dataTable.ajax.reload(() => {
            $('.loading').hide();
        });
    });
});