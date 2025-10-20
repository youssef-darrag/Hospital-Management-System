$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);

        const swal = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-danger mx-2",
                cancelButton: "btn btn-light"
            },
            buttonsStyling: false
        });

        swal.fire({
            title: "Are you sure that you want to delete this record?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            cancelButtonText: "No, cancel!",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/Admin/${btn.data('controller')}/Delete/${btn.data('id')}`,
                    method: 'DELETE',
                    success: function (res) {
                        if (res.redirectUrl) {
                            window.location.href = res.redirectUrl;
                        }
                        else {
                            swal.fire({
                                title: "Deleted!",
                                text: "The record has been deleted.",
                                icon: "success"
                            });

                            btn.parents('tr').fadeOut();
                        }
                    },
                    error: function () {
                        swal.fire({
                            title: "Oooops...",
                            text: "Something went wrong.",
                            icon: "error"
                        });
                    }
                });
            }
        });
    });
});