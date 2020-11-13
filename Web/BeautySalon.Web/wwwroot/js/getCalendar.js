function getCalendar() {
    var events = [];

    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "/Stylists/Appointments/GetAppointments",
            success: function (data) {
                data.appointments.forEach(appointment => {
                    events.push({
                        title: appointment.procedureName + " - " + appointment.clientFullName,
                        start: appointment.formattedStart,
                        end: appointment.formattedEnd,
                        url: `https://localhost:44319/Stylists/Appointments/GetInfoCurrentAppointment/${appointment.id}`,
                    });
                });
                generateCalender(events);
            },
        });

        function generateCalender(events) {
            var divCalender = document.getElementById("calendar");
            var calendar = new FullCalendar.Calendar(divCalender, {
                height: 'auto',
                headerToolbar: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                initialDate: new Date(),
                navLinks: true, // can click day/week names to navigate views
                selectable: true,
                selectMirror: true,
                editable: true,
                dayMaxEvents: true, // allow "more" link when too many events
                hiddenDays: [0],
                events: events,
                selectAllow: function (select) {
                    return moment().diff(select.start) <= 0
                },
                eventClick: function (info) {
                    console.log(info.event.url);
                },
                eventDidMount: function (info) {
                    $(info.el).tooltip({
                        title: info.event.title,
                        placement: "top",
                        trigger: "hover",
                        container: "body"
                    })
                }
            });
            calendar.render();
        };
    });
};
