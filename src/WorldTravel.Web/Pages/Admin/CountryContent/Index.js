$(function () {
    var l = abp.localization.getResource('WorldTravel');
    var createModal = new abp.ModalManager(abp.appPath + 'Admin/CountryContent/Create');
    var editModal = new abp.ModalManager(abp.appPath + 'Admin/CountryContent/Edit');

    var getFilter = function () {
        return {
            countryNameFilter: $("#CountryNameFilter").val()
        };
    };

    var dataTable = $('#CountryContentsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(worldTravel.services.countryContent.getCountryContentList, getFilter),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    //visible: abp.auth.isGranted('WorldTravel.Forms.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    //visible: abp.auth.isGranted('WorldTravel.Forms.Delete'),
                                    confirmMessage: function (data) {
                                        return l('DeleteConfirmMessage', data.record.name);
                                    },
                                    action: function (data) {
                                        worldTravel.services.countryContent.softDelete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
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
                    title: l('MainImage'),
                    data: "previewImageUrl",
                    render: function (data) {
                        return applyImage_h(data);
                    }
                },
                {
                    title: l('Title'),
                    data: "title",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('ShortDescription'),
                    data: "shortDescription",
                    render: function (data) {
                        return applyShortening(data);
                    }
                },
                {
                    title: l('Description'),
                    data: "description",
                    render: function (data) {
                        return applyShortening(data);
                    }
                },
                {
                    title: l('ExtraDescription'),
                    data: "extraDescription",
                    render: function (data) {
                        return applyShortening(data);
                    }
                },
                {
                    title: l('ReadCount'),
                    data: "readCount",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('Rank'),
                    data: "rank",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('IsSeenHomePage'),
                    data: "isSeenHomePage",
                    render: function (data) {
                        if (data === true) {
                            return l('True');
                        }
                        return l('False');
                    }
                },
                {
                    title: l('TotalImageCount'),
                    data: "totalImageCount",
                    render: function (data) {
                        return data;
                    }
                },
                {
                    title: l('TotalVideoCount'),
                    data: "totalVideoCount",
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
                }
            ],
            createdRow: function (nRow, aData) {
            }
        })
    );


    createModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.info(l('SuccessfullyCompleted'));
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.info(l('SuccessfullyCompleted'));
    });

    $('#createCountryContentButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $('#btnSearch').on('click', function (e) {
        $('.loading').show();

        dataTable.ajax.reload(() => {
            $('.loading').hide();
        });
    });
});