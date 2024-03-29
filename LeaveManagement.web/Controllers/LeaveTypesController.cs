﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.web.Data;
using AutoMapper;
using LeaveManagement.web.Models;
using LeaveManagement.web.Contracts;
using Microsoft.AspNetCore.Authorization;
using LeaveManagement.web.Constants;

namespace LeaveManagement.web.Controllers
{
    [Authorize(Roles = UserRole.Administrator)]
    public class LeaveTypesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public LeaveTypesController(IMapper mapper, ILeaveTypeRepository leaveTypeRepository, ILeaveAllocationRepository leaveAllocationRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveAllocationRepository = leaveAllocationRepository;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _leaveTypeRepository.GetAllAsync();
            if (leaveTypes != null)
            {
                var leaveTypesVM = _mapper.Map<List<LeaveTypeVM>>(leaveTypes);
                return View(leaveTypesVM);
            }

            return Problem("Entity set 'ApplicationDbContext.LeaveTypes'  is null.");
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var leaveType = await _leaveTypeRepository.GetAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<LeaveTypeVM>(leaveType));
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,DefaultDays")] CreateLeaveTypeVM leaveTypeVM)
        {
            if (ModelState.IsValid)
            {
                var leaveType = _mapper.Map<LeaveType>(leaveTypeVM);
                await _leaveTypeRepository.CreateAsync(leaveType);
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeVM);
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypeRepository.GetAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<EditLeaveTypeVM>(leaveType));
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,DefaultDays,Id")] EditLeaveTypeVM leaveTypeVM)
        {
            if (id != leaveTypeVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var leaveType = _mapper.Map<LeaveType>(leaveTypeVM);
                    await _leaveTypeRepository.UpdateAsync(leaveType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _leaveTypeRepository.Exists(leaveTypeVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeVM);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = _leaveTypeRepository.GetAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            return View(await leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _leaveTypeRepository.GetAsync(id);
            if (leaveType != null)
            {
                await _leaveTypeRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveTypes/AllocateLeave/5
        public async Task<IActionResult> AllocateLeave(int id)
        {
            //await _leaveAllocationRepository.LeaveAllocation(id);
            var leaveType = await _leaveTypeRepository.GetAsync(id);
            if (leaveType != null)
            {
                RedirectToAction(nameof(Index));
            }

            return View(leaveType);
        }

        // POST: LeaveTypes/AllocateLeave/5
        [HttpPost, ActionName("AllocateLeave")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeaveConfirmed(int id)
        {
            var leaveType = await _leaveTypeRepository.GetAsync(id);
            if (leaveType != null)
            {
                await _leaveAllocationRepository.LeaveAllocation(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
