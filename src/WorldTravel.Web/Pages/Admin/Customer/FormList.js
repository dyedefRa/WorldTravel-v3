$(function () {

    var l = abp.localization.getResource('WorldTravel');
    var userId = document.getElementById("userId");
    var getFilter = function () {
        return {
            id: userId.value
        };
    };

    var editModal = new abp.ModalManager(abp.appPath + 'Admin/Customer/FormEditModal');

    var dataTable = $('#FormTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(worldTravel.services.form.getFormListAsyncByUserId, getFilter),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                //{
                                //    text: l('Detay'),
                                //    action: function (data) {
                                //        if (data.record.userName === 'admin') { //admin ise uyarı ver.
                                //            abp.notify.warn("Bu kullanıcıyı göremezsiniz.");
                                //        }
                                //        else {
                                //            window.location.href = '/Admin/Customer/FormDetail?id=' + data.record.id;
                                //        }

                                //    },
                                //},
                                {
                                    text: l('Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                            ]
                    }
                },
                {
                    title: l('CountryName'),
                    data: "countryName",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('Name'),
                    data: "name",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('FormRegType'),
                    data: "formRegType",
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
                    title: l('Uygunluk'),
                    data: "formIsOk",
                    render: function (data) {
                        return data;
                    }
                },
            ],
            createdRow: function (nRow, aData) {
            }
        })
    );

    editModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.info(l('SuccessfullyCompleted'));
    });

    $(function () {
        _dataTable.ajax.reload();
    });

    $('#btnSearch').on('click', function (e) {
        $('.loading').show();

        dataTable.ajax.reload(() => {
            $('.loading').hide();
        });
    });
});