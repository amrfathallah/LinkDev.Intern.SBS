import { Component, Input, OnInit } from '@angular/core';
import { GetResourceDto } from '../models/resource/get-resources.dto';
import { ActivatedRoute, Route } from '@angular/router';
import { AdminService } from '../services/admin-service';

@Component({
  selector: 'app-resource-booking',
  templateUrl: './resource-page.component.html',
  styleUrls: ['./resource-page.component.css'],
})
export class ResourceDetailsComponent implements OnInit {
  @Input() resource!: GetResourceDto;
  @Input() date!: string;

  id!: string;
  timeSlots: string[] = [];
  selectedSlots: string[] = [];
  selectedDate!: Date;

  constructor(
    private router: ActivatedRoute,
    private adminService: AdminService
  ) {}

  ngOnInit(): void {
    const dateStr = this.router.snapshot.paramMap.get('date');
    this.selectedDate = dateStr ? new Date(dateStr) : new Date();
    console.log('Selected date:', this.selectedDate.toString());
    this.id = this.router.snapshot.paramMap.get('id')!;
    /*if (this.id) {
      this.adminService.getResourceById(this.id).subscribe({
        next: (data) => (this.resource = data),
        error: (err) => console.error('Failed to fetch resource', err),
      });
    }*/

    this.resource = {
      id: '1',
      name: 'Conference Room A',
      typeId: 1,
      type: 'room',
      capacity: 12,
      isActive: true,
      openAt: '09:00:00',
      closeAt: '17:00:00',
    };

    this.generateTimeSlots(this.resource.openAt, this.resource.closeAt);
  }

  generateTimeSlots(start: string, end: string): void {
    const timeSlots: string[] = [];

    const [startHours, startMinutes] = start.split(':').map(Number);
    const [endHours, endMinutes] = end.split(':').map(Number);

    let current = new Date();
    current.setHours(startHours, startMinutes, 0, 0);

    const endTime = new Date();
    endTime.setHours(endHours, endMinutes, 0, 0);

    while (current < endTime) {
      const hours = current.getHours().toString().padStart(2, '0');
      const minutes = current.getMinutes().toString().padStart(2, '0');
      timeSlots.push(`${hours}:${minutes}`);

      current.setMinutes(current.getMinutes() + 30);
    }

    this.timeSlots = timeSlots;
  }

  getSlotEndTime(startTime: string): string {
    const [hours, minutes] = startTime.split(':').map(Number);
    const start = new Date();
    start.setHours(hours, minutes, 0, 0);
    start.setMinutes(start.getMinutes() + 30);

    const endHours = start.getHours().toString().padStart(2, '0');
    const endMinutes = start.getMinutes().toString().padStart(2, '0');
    return `${endHours}:${endMinutes}`;
  }

  onSlotClick(slot: string): void {
    if (!this.resource.isActive) return;

    const index = this.selectedSlots.indexOf(slot);

    if (index > -1) {
      // Allow deselection freely
      this.selectedSlots.splice(index, 1);
      return;
    }

    if (this.selectedSlots.length === 0) {
      this.selectedSlots.push(slot);
      return;
    }

    const lastSlot = this.selectedSlots[this.selectedSlots.length - 1];
    const lastSlotTime = this.parseTime(lastSlot);
    const currentSlotTime = this.parseTime(slot);

    const diff =
      (currentSlotTime.getTime() - lastSlotTime.getTime()) / (1000 * 60);

    if (diff === 30) {
      this.selectedSlots.push(slot);
    } else {
      alert('You can only select consecutive slots.');
    }
  }

  parseTime(timeStr: string): Date {
    const [hours, minutes] = timeStr.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return date;
  }

  areSlotsConsecutive(): boolean {
    if (this.selectedSlots.length <= 1) return true;

    // Parse times to Date objects
    const parsedDates = this.selectedSlots.map((timeStr) => {
      const [hours, minutes] = timeStr.split(':').map(Number);
      if (isNaN(hours) || isNaN(minutes)) {
        throw new Error(`Invalid slot format: ${timeStr}`);
      }
      const date = new Date();
      date.setHours(hours, minutes, 0, 0);
      return date;
    });

    // Sort times ascending
    parsedDates.sort((a, b) => a.getTime() - b.getTime());

    // Check each slot is exactly 30 minutes after the previous
    for (let i = 0; i < parsedDates.length - 1; i++) {
      const diffInMs = parsedDates[i + 1].getTime() - parsedDates[i].getTime();
      if (diffInMs !== 30 * 60 * 1000) {
        return false;
      }
    }

    return true;
  }

  checkout(): void {
    if (this.selectedSlots.length === 0) {
      alert('No slots selected.');
      return;
    }

    if (!this.areSlotsConsecutive()) {
      alert('Selected slots are not consecutive.');
      return;
    }
  }
}
