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
    // ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "EngineerName"); //Passes a dropdown of engineers into the form
    return View();
}

[HttpPost] //Actually creates the machine that was just entered
public ActionResult Create(Machine machine) //int EngineerId
{
    _db.Machines.Add(machine); //adds the new machine to the machine database
    // if (EngineerId != 0) //Basically means: if an engineer was selected in the form add it to the join table as well??
    // {
    //     _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
    // }
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
    // ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "EngineerName"); //passes in a dropdown list of engineers to choose from
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
    var foundMachine = _db.Machines.FirstOrDefault(machines => machines.MachineId == id);
    return View(foundMachine);
}

[HttpPost, ActionName("Delete")] //What is this second parameter??
public ActionResult DeleteConfirmed(int id)
{
    var thisMachine = _db.Machines.FirstOrDefault(machines => machines.MachineId == id);
    _db.Machines.Remove(thisMachine);
    _db.SaveChanges();
    return RedirectToAction("Index");
}

[HttpPost]
public ActionResult DeleteEngineer(int joinId)
{
    var joinEntry = _db.EngineerMachine.FirstOrDefault(entry => entry.EngineerMachineId == joinId); //when a machine is deleted it deletes the association of any engineers to the machine but not the engineer themself.
    _db.EngineerMachine.Remove(joinEntry);
    _db.SaveChanges();
    return RedirectToAction("Index");
}
    [HttpPost]
    public ActionResult Index(string MachineName)
    {
      List<Machine> model = _db.Machines.Where(x => x.MachineName.Contains(MachineName)).ToList();
      List<Machine> SortedList = model.OrderBy(o => o.MachineName).ToList();
      return View("Index", SortedList);
    }
  }
}