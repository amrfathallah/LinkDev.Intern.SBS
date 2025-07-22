import { Component, Input, OnInit } from '@angular/core';
import { GetResourceDto } from '../models/resource/get-resources.dto';
import { ActivatedRoute, Route } from '@angular/router';
import { AdminService } from '../services/admin-service';
import { SlotDto } from '../models/slot.dto';

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
  slots: SlotDto[] = [];
  selectedSlots: SlotDto[] = [];
  selectedDate!: Date;

  constructor(
    private router: ActivatedRoute,
    private adminService: AdminService
  ) {}

  ngOnInit(): void {
    const dateStr = this.router.snapshot.paramMap.get('date');
    this.selectedDate = dateStr ? new Date(dateStr) : new Date();
    this.id = this.router.snapshot.paramMap.get('id')!;
    if (this.id) {
      this.adminService.getResourceById(this.id).subscribe({
        next: (data) => (this.resource = data),
        error: (err) => console.error('Failed to fetch resource', err),
      });
    }

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
    this.loadSlots();
  }

  loadSlots(): void {
    const resourceId = this.resource.id;
    const date = this.selectedDate.toISOString();

    this.adminService.getBookedSlots(resourceId, date).subscribe({
      next: (rawSlots) => {
        const bookedSlots: SlotDto[] = rawSlots.map((slot: any) => ({
          id: slot.id,
          startTime: slot.startTime,
          endTime: slot.endTime,
          isActive: slot.isActive,
          isBooked: true,
        }));

        this.generateTimeSlots(
          this.resource.openAt,
          this.resource.closeAt,
          bookedSlots
        );
      },
      error: (err) => {
        this.generateTimeSlots(this.resource.openAt, this.resource.closeAt, []);
      },
    });
  }

  generateTimeSlots(start: string, end: string, bookedSlots: SlotDto[]): void {
    this.slots = []; // reset

    const [startHours, startMinutes] = start.split(':').map(Number);
    const [endHours, endMinutes] = end.split(':').map(Number);

    let current = new Date();
    current.setHours(startHours, startMinutes, 0, 0);

    const endTime = new Date();
    endTime.setHours(endHours, endMinutes, 0, 0);

    let idCounter = 1;

    while (current < endTime) {
      const slotStartStr = current.toTimeString().substring(0, 5);

      const slotEnd = new Date(current.getTime());
      slotEnd.setMinutes(slotEnd.getMinutes() + 30);
      const slotEndStr = slotEnd.toTimeString().substring(0, 5);

      const isBooked = bookedSlots.some(
        (slot) => slot.startTime === slotStartStr
      );

      this.slots.push({
        id: idCounter.toString(),
        startTime: slotStartStr,
        endTime: slotEndStr,
        isActive: true,
        isBooked: isBooked,
      });

      current.setMinutes(current.getMinutes() + 30);
      idCounter++;
    }
  }

  /*getSlotEndTime(startTime: string): string {
    const [hours, minutes] = startTime.split(':').map(Number);
    const start = new Date();
    start.setHours(hours, minutes, 0, 0);
    start.setMinutes(start.getMinutes() + 30);

    const endHours = start.getHours().toString().padStart(2, '0');
    const endMinutes = start.getMinutes().toString().padStart(2, '0');
    return `${endHours}:${endMinutes}`;
  }*/

  onSlotClick(slot: SlotDto): void {
    if (!this.resource.isActive || !slot.isActive || slot.isBooked) return;

    const index = this.selectedSlots.findIndex(
      (s) => s.startTime === slot.startTime
    );

    if (index > -1) {
      this.selectedSlots.splice(index, 1); // Deselect
      return;
    }

    if (this.selectedSlots.length === 0) {
      this.selectedSlots.push(slot);
      return;
    }

    const lastSlot = this.selectedSlots[this.selectedSlots.length - 1];
    const lastSlotTime = this.parseTime(lastSlot.startTime);
    const currentSlotTime = this.parseTime(slot.startTime);

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

    const parsedDates = this.selectedSlots.map((slot) => {
      const [hours, minutes] = slot.startTime.split(':').map(Number);
      const date = new Date();
      date.setHours(hours, minutes, 0, 0);
      return date;
    });

    parsedDates.sort((a, b) => a.getTime() - b.getTime());

    for (let i = 0; i < parsedDates.length - 1; i++) {
      const diff = parsedDates[i + 1].getTime() - parsedDates[i].getTime();
      if (diff !== 30 * 60 * 1000) return false;
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
