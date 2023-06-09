﻿using AWWW_Lab1.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace AWWW_Lab1.Controllers {
    public class TeamsController : Controller {
        private AppDbContext _context;
        public TeamsController(AppDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var teams = await _context.Teams
                .Include(x => x.League)
                .ToListAsync();
            return View(teams);
        }

        public async Task<IActionResult> AddNew() {
            var leagues = await _context.Leagues.ToListAsync();
            ViewBag.leagues = leagues;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNew(TeamVM teamvm) {
            League league = await _context.Leagues.FirstOrDefaultAsync(x => x.Id == teamvm.LeagueId);
            Team team = new Team {
                City = teamvm.City,
                Country = teamvm.Country,
                FoundingDate = teamvm.FoundingDate,
                Name = teamvm.Name,
                League = league
            };

            _context.Teams.Add(team);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            var task = await _context.Teams
                .Where(x => x.Id == id)
                .Include(x => x.League)
                .FirstOrDefaultAsync();
            var leagues = await _context.Leagues.ToListAsync();
            ViewBag.leagues = leagues;
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeamVM teamvm) {
            Team? team = _context.Teams
                .Include(x => x.League)
                .FirstOrDefault(x => x.Id == teamvm.Id);

            if (team == null)
                return RedirectToAction("Index");

            team.Name = teamvm.Name;
            team.City = teamvm.City;
            team.Country = teamvm.Country;
            team.FoundingDate = teamvm.FoundingDate;
            team.League = await _context.Leagues.FirstOrDefaultAsync(x => x.Id == teamvm.LeagueId);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
