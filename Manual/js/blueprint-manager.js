var oldTemplateID = null;

$(document).ready(function()
{
  var templateSelect = $("#template-select");

  templateSelect.change(function()
  {
    var templateID = $(this).val();

    if (oldTemplateID != null)
    {
      $(oldTemplateID).hide();
    }

    $(templateID).show();
    oldTemplateID = templateID;
  });

  $(".template").each(function()
  {
    templateSelect.append($("<option>",
    {
      value: "#" + $(this).attr("id"),
      text: $(this).attr("id")
    }));
  });

  var firstTemplate = $(".template").first();

  firstTemplate.show();
  oldTemplateID = "#" + firstTemplate.attr("id");

  $(".piece").each(function()
  {
    $(this).data("default-transform", $(this).attr("transform"));
  });

  $("#reset").click(function()
  {
    $(".piece").each(function()
    {
      $(this).attr("transform", $(this).data("default-transform"));
    });
  });
});
