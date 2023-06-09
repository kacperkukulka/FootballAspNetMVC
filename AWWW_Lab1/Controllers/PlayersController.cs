﻿using Microsoft.AspNetCore.Mvc;
using AWWW_Lab1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AWWW_Lab1.Controllers {
    public class PlayersController : Controller {
        private AppDbContext _context;
        public PlayersController(AppDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var players = await _context.Players
                .Include(x => x.Positions)
                .Include(x => x.Team)
                .ToListAsync();
            return View(players); 
        }

        public async Task<IActionResult> AddNew() {
            var positions = await _context.Positions.ToListAsync();
            ViewBag.positions = new SelectList(positions, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNew(PlayerVM playervm) {
            Player player = new Player(playervm);

            player.Positions = new List<Position>();
            if (playervm.PositionsId != null) {
                foreach (var position in playervm.PositionsId) {
                    var positionAdd = await _context.Positions.FirstOrDefaultAsync(pos => pos.Id == position);
                    player.Positions.Add(positionAdd);
                }
            }

            _context.Players.Add(player);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Info(int id) {
            var player = await _context.Players
                .Where(x => x.Id == id)
                .Include(x => x.Positions)
                .SingleOrDefaultAsync();
            if (player == null)
                return RedirectToAction("Index");
            ViewBag.positions = new SelectList(player.Positions, "Id", "Name");
            return View(player);
        }

        public async Task<IActionResult> EditTeam(int id) {
            var player = await _context.Players
                .Where(x => x.Id == id)
                .Include(x => x.Team)
                .SingleOrDefaultAsync();
            ViewBag.teams = await _context.Teams.ToListAsync();
            return View(player);
        }

        public async Task<IActionResult> DeleteTeam(int id) {
            var player = await _context.Players
                .Where(x => x.Id == id)
                .Include(x => x.Team)
                .SingleOrDefaultAsync();
            player.Team = null;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddTeam(int id, int TeamId) {
            var player = await _context.Players
                .Where(x => x.Id == id)
                .Include(x => x.Team)
                .SingleOrDefaultAsync();
            player!.Team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == TeamId);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            var player = await _context.Players
                .Where(x => x.Id == id)
                .Include(x => x.Positions)
                .SingleOrDefaultAsync();
            ViewBag.positions = await _context.Positions.ToListAsync();
            return View(player);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlayerVM playervm) {
            Player? player = _context.Players
                .Include(x => x.Positions)
                .FirstOrDefault(x => x.Id == playervm.Id);

            if (player == null)
                return RedirectToAction("Index");

            player.FirstName = playervm.FirstName;
            player.LastName = playervm.LastName;
            player.BirthDate = playervm.BirthDate;
            player.Country = playervm.Country;

            player.Positions.Clear();
            player.Positions = new List<Position>();
            if (playervm.PositionsId != null) {
                foreach (var position in playervm.PositionsId) {
                    var positionAdd = await _context.Positions.FirstOrDefaultAsync(pos => pos.Id == position);
                    player.Positions.Add(positionAdd);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
