$(document).ready(function () {
    $("#StartTime").on("change", function () {
        var start = $("#StartTime option:selected").text().trim(); // النص "10:00 AM"
        var duration = parseInt($("#Duration").val());

        if (!start || isNaN(duration)) {
            console.warn("Invalid StartTime or Duration");
            return;
        }

        var m = start.match(/^(\d{1,2}):(\d{2})(?:\s*(AM|PM))$/i);
        if (!m) {
            console.warn("Unsupported time format:", start);
            $("#EndTime").val("");
            return;
        }

        var hours = parseInt(m[1], 10);
        var minutes = parseInt(m[2], 10);
        var period = m[3].toUpperCase();

        // Convert to 24h
        if (period === "PM" && hours < 12) hours += 12;
        if (period === "AM" && hours === 12) hours = 0;

        var totalMinutes = hours * 60 + minutes + duration;
        var endHours = Math.floor(totalMinutes / 60) % 24;
        var endMinutes = totalMinutes % 60;

        var formattedHours = endHours % 12 || 12;
        var formattedMinutes = endMinutes.toString().padStart(2, "0");
        var endPeriod = endHours >= 12 ? "PM" : "AM";

        var endTime = formattedHours + ":" + formattedMinutes + " " + endPeriod;

        $("#EndTime").val(endTime);
    });
});