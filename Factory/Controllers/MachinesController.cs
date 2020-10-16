using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController(FactoryContext db) //establishes reference to database
    {
      _db = db;
    }

    public ActionResult Index() //Shows screen with list of machines
    {
      return View(_db.Machines.ToList());
    }

public ActionResult Create() //shows page to create a new machine
{
    ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "EngineerName"); //Passes a dropdown of engineers into the form
    return View();
}

[HttpPost] //Actually creates the machine that was just entered
public ActionResult Create(Machine machine, int EngineerId)
{
    _db.Machines.Add(machine); //adds the new machine to the machine database
    if (EngineerId != 0) //Basically means: if an engineer was selected in the form add it to the join table as well??
    {
        _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
}

    public ActionResult Details(int id) //returns a page with the info for a specific machine
    {
        var thisMachineInfo = _db.Machines //gets the machine database
        .Include(machine => machine.Engineers) //includes the engineers field for the machine
        .ThenInclude(join => join.Engineer) //retrieves the specific engineer info
        .FirstOrDefault(machine => machine.MachineId == id); //finds the specific machine to display info on
    return View(thisMachineInfo);
    }

public ActionResult Edit(int id)
{
    var foundMachine = _db.Machines.FirstOrDefault(machines => machines.MachineId == id); //finds the machine to be edited by matching the id's
    ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "EngineerName"); //passes in a dropdown list of engineers to choose from
    return View(foundMachine);
}

    [HttpPost]
    public ActionResult Edit(Machine machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
      }
      _db.Entry(machine).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddEngineer(int id) //Displays page that allows you to add an additional engineer to the machine
{
    var thisMachine = _db.Machines.FirstOrDefault(machines => machines.MachineId == id); //finds the machine to have an engineer added to
    ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "EngineerName"); //Passes in a dropdown to delect an engineer
    return View(thisMachine);
}

[HttpPost]
public ActionResult AddEngineer(Machine machine, int EngineerId)
{
    if (EngineerId != 0)
    {
    _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
}

public ActionResult Delete(int id)
{
    var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    return View(thisItem);
}

[HttpPost, ActionName("Delete")]
public ActionResult DeleteConfirmed(int id)
{
    var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    _db.Items.Remove(thisItem);
    _db.SaveChanges();
    return RedirectToAction("Index");
}

[HttpPost]
public ActionResult DeleteCategory(int joinId)
{
    var joinEntry = _db.CategoryItem.FirstOrDefault(entry => entry.CategoryItemId == joinId);
    _db.CategoryItem.Remove(joinEntry);
    _db.SaveChanges();
    return RedirectToAction("Index");
}

  }
}