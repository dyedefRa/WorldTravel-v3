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
                                {
                                    text: l('Detay'),
                                    visible: function (data) {
                                        console.log(data);
                                        return true;
                                        //    return abp.auth.isGranted('AbpIdentity.Users.Detail') /*&& data.userName !== 'admin'*/;
                                    },
                                    action: function (data) {
                                        if (data.record.userName === 'admin') { //admin ise uyarı ver.
                                            abp.notify.warn("Bu kullanıcıyı göremezsiniz.");
                                        }
                                        else {
                                            window.location.href = '/Admin/Customer/FormDetail?id=' + data.record.id;
                                        }

                                    },
                                },
                                {
                                    text: l('Edit'),
                                    //visible: abp.auth.isGranted('WorldTravel.Forms.Edit'),
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
                //{
                //    title: l('MainImage'),
                //    data: "imageUrl",
                //    render: function (data) {
                //        return applyImage_h(data);
                //    }
                //},
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
                //{
                //    title: l('Surname'),
                //    data: "surname",
                //    render: function (data) {
                //        return data;
                //    }
                //},
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
                //{
                //    title: l('Description'),
                //    data: "description",
                //    render: function (data) {
                //        return applyShortening(data);
                //    }
                //},
                //{
                //    title: l('ExtraDescription'),
                //    data: "extraDescription",
                //    render: function (data) {
                //        return applyShortening(data);
                //    }
                //},
                //{
                //    title: l('ReadCount'),
                //    data: "readCount",
                //    render: function (data) {
                //        return data;
                //    }
                //},
                //{
                //    title: l('Rank'),
                //    data: "rank",
                //    render: function (data) {
                //        return data;
                //    }
                //},
                //{
                //    title: l('IsSeenHomePage'),
                //    data: "isSeenHomePage",
                //    render: function (data) {
                //        if (data === true) {
                //            return l('True');
                //        }
                //        return l('False');
                //    }
                //},
                //{
                //    title: l('TotalImageCount'),
                //    data: "totalImageCount",
                //    render: function (data) {
                //        return data;
                //    }
                //},
                //{
                //    title: l('TotalVideoCount'),
                //    data: "totalVideoCount",
                //    render: function (data) {
                //        return data;
                //    }
                //},
                //{
                //    title: l('CreatedDate'),
                //    data: "creationTime",
                //    render: function (data) {
                //        return setDate(data);
                //    }
                //}
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