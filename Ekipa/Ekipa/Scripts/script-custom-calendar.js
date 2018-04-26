$(document).ready(function () {
	$('#calendar').fullCalendar({
		header:
			{
				left: 'prev,next today',
				center: 'title',
				right: 'month,agendaWeek,agendaDay'
			},
		buttonText: {
			today: 'today',
			month: 'month',
			week: 'week',
			day: 'day'
		},

		events: function (start, end, timezone, callback) {
			$.ajax({
				url: '/Calendar/GetCalendarData',
				type: "GET",
				dataType: "JSON",

				success: function (result) {
					var events = [];

					$.each(result, function (i, data) {
						events.push(
							{
								start: moment(data.DateStart).format('YYYY-MM-DD'),
								end: moment(data.DateStop).format('YYYY-MM-DD'),
								backgroundColor: "#9501fc",
								borderColor: "#fc0101"
							});
					});

					callback(events);
				}
			});
		},

		eventRender: function (event, element) {
			element.qtip(
				{
					content: event.description
				});
		},

		editable: false
	});
});  