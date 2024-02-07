$(function () {
    var l = abp.localization.getResource('WorldTravel');
    var createModal = new abp.ModalManager(abp.appPath + 'Admin/Blog/Create');
    var editModal = new abp.ModalManager(abp.appPath + 'Admin/Blog/Edit');
    var getFilter = function () {
        return {
            blogTitleFilter: $("#BlogTitleFilter").val()
        };
    };
    var dataTable = $('#PostTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(worldTravel.services.blog.getBlogList, getFilter),
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
                                        worldTravel.services.blog.softDelete(data.record.id)
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
                    title: l('Title'),
                    data: "title",
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

    $('#createBlogButton').click(function (e) {
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