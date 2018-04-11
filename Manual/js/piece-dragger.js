SVGElement.prototype.getTransformToElement = SVGElement.prototype.getTransformToElement || function(elem)
{
    return elem.getScreenCTM().inverse().multiply(this.getScreenCTM());
};

var mainSVG = null;
var selectedElement = null;
var globalPoint = null;
var currentMatrix = null;

$(document).ready(function()
{
  mainSVG = document.getElementById("mainSVG");

  $(".piece").click(function(event)
  {
    if (event.ctrlKey || event.metaKey)
    {
      selectedElement = this;

      if ($(selectedElement).attr("transform") == null)
      {
        $(selectedElement).attr("transform", "matrix(1 0 0 1 0 0))");
      }

      var width = parseFloat($(selectedElement).attr("width"));
      var height = parseFloat($(selectedElement).attr("height"));

      currentMatrix = $(selectedElement).attr("transform").slice(7, -1).split(' ');
      for (var i=0; i < currentMatrix.length; i++)
      {
        currentMatrix[i] = parseFloat(currentMatrix[i]);
      }

      //To 90deg
      if (currentMatrix[0] == 1)
      {
        currentMatrix[0] = 0;
        currentMatrix[1] = 1;
        currentMatrix[2] = -1;
        currentMatrix[3] = 0;
        currentMatrix[4] += (width + height) / 2;
        currentMatrix[5] += (height - width) / 2;
      }
      //To 270deg
      else if (currentMatrix[0] == -1)
      {
        currentMatrix[0] = 0;
        currentMatrix[1] = -1;
        currentMatrix[2] = 1;
        currentMatrix[3] = 0;
        currentMatrix[4] += (-width - height) / 2;
        currentMatrix[5] += (width - height) / 2;
      }
      //To 0deg
      else if (currentMatrix[2] == 1)
      {
        currentMatrix[0] = 1;
        currentMatrix[1] = 0;
        currentMatrix[2] = 0;
        currentMatrix[3] = 1;        
        currentMatrix[4] += (height - width) / 2;
        currentMatrix[5] += (-width - height) / 2;
      }
      //To 180deg
      else
      {
        currentMatrix[0] = -1;
        currentMatrix[1] = 0;
        currentMatrix[2] = 0;
        currentMatrix[3] = -1;
        currentMatrix[4] += (width - height) / 2;
        currentMatrix[5] += (width + height) / 2;
      }

      newTransform = "matrix(" + currentMatrix.join(' ') + ")";
      $(selectedElement).attr("transform", newTransform);
    }
  });

  $(".piece").mousedown(function(event)
  {
    selectedElement = this;

    globalPoint = mainSVG.createSVGPoint();
    globalPoint.x = event.clientX;
    globalPoint.y = event.clientY;

    var ctm = mainSVG.getScreenCTM();
    ctm = ctm.inverse();
    globalPoint = globalPoint.matrixTransform(ctm);

    if ($(selectedElement).attr("transform") == null)
    {
      $(selectedElement).attr("transform", "matrix(1 0 0 1 0 0))");
    }

    currentMatrix = $(selectedElement).attr("transform").slice(7, -1).split(' ');
    for (var i=0; i < currentMatrix.length; i++)
    {
      currentMatrix[i] = parseFloat(currentMatrix[i]);
    }

    $(mainSVG).on("mousemove", function(event)
    {
      var newGlobalPoint = mainSVG.createSVGPoint();
      newGlobalPoint.x = event.clientX;
      newGlobalPoint.y = event.clientY;

      var ctm = mainSVG.getScreenCTM();
      ctm = ctm.inverse();
      newGlobalPoint = newGlobalPoint.matrixTransform(ctm);

      dx = newGlobalPoint.x - globalPoint.x;
      dy = newGlobalPoint.y - globalPoint.y;

      dx = Math.round(dx / 20) * 20;
      dy = Math.round(dy / 20) * 20;

      globalPoint.x += dx;
      globalPoint.y += dy;

      currentMatrix[4] += dx;
      currentMatrix[5] += dy;
      newTransform = "matrix(" + currentMatrix.join(' ') + ")";
      $(selectedElement).attr("transform", newTransform);
    });

    $(mainSVG).on("mouseup", function()
    {
      $(mainSVG).off("mousemove").off("mouseup");
      selectedElement = null;
    });
  });
});
