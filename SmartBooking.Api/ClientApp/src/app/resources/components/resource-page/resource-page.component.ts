
import { Component, Input, OnInit } from '@angular/core';
import { GetResourceDto } from '../../models/dtos/get-resources.dto';
import { ActivatedRoute, Route } from '@angular/router';
import { SlotDto } from '../../models/dtos/slot.dto';
import { ResourceService } from '../../services/resource-service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-resource-booking',
  templateUrl: './resource-page.component.html',
  styleUrls: ['./resource-page.component.css'],
})
export class ResourceDetailsComponent implements OnInit {
  @Input() resource: GetResourceDto = {} as GetResourceDto;
  @Input() date!: string;

  id!: string;
  timeSlots: string[] = [];
  slots: SlotDto[] = [];
  selectedSlots: SlotDto[] = [];
  selectedDate!: Date;

  constructor(
    private router: ActivatedRoute,
    private resourceService: ResourceService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const dateStr = this.router.snapshot.paramMap.get('date');
    this.selectedDate = dateStr ? new Date(dateStr) : new Date();
    this.id = this.router.snapshot.paramMap.get('id')!;
    if (this.id) {
      this.resourceService.getResourceById(this.id).subscribe({
        next: (data) => (this.resource = data,this.loadSlots()),
        error: (err) => console.error('Failed to fetch resource', err),
      });
    }
  } 

  loadSlots(): void {
    // Format date as YYYY-MM-DD for the API
    const date = this.selectedDate.toISOString().split('T')[0];


    this.resourceService.getBookedSlots(this.id, date).subscribe({
      next: (response) => {
        const bookedSlots: SlotDto[] = response.bookedSlots.map((slot) => ({
          id: slot.slotId.toString(),
          startTime: slot.startTime.substring(0, 5), 
          endTime: slot.endTime.substring(0, 5), 
          isActive: true,
          isBooked: true,
        }));

        this.generateTimeSlots(
          this.resource.openAt,
          this.resource.closeAt,
          bookedSlots
        );
      },
      error: (err) => {
        console.error('Failed to load booked slots:', err);
        console.error('Error details:', err.error);
        this.snackBar.open('Failed to load booked slots. Showing all slots as available.', 'Close', {
          duration: 4000,
          panelClass: ['error-snackbar']
        });
        // Generate slots without any booked slots
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
        (slot) => {
          // Normalize both time formats to HH:MM for comparison
          const bookedSlotTime = slot.startTime.substring(0, 5); // Remove seconds if present
          return bookedSlotTime === slotStartStr;
        }
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
    if (!this.resource.isActive || !slot.isActive) {
      this.snackBar.open('This resource is not available', 'Close', {
        duration: 3000,
        panelClass: ['warning-snackbar']
      });
      return;
    }

    if (slot.isBooked) {
      this.snackBar.open(`Slot ${slot.startTime} - ${slot.endTime} is already reserved`, 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    const index = this.selectedSlots.findIndex(
      (s) => s.startTime === slot.startTime
    );

    if (index > -1) {
      this.selectedSlots.splice(index, 1); // Deselect
      this.snackBar.open(`Slot ${slot.startTime} - ${slot.endTime} deselected`, 'Close', {
        duration: 2000,
        panelClass: ['info-snackbar']
      });
      return;
    }

    if (this.selectedSlots.length === 0) {
      this.selectedSlots.push(slot);
      this.snackBar.open(`Slot ${slot.startTime} - ${slot.endTime} selected`, 'Close', {
        duration: 2000,
        panelClass: ['success-snackbar']
      });
      return;
    }

    const lastSlot = this.selectedSlots[this.selectedSlots.length - 1];
    const lastSlotTime = this.parseTime(lastSlot.startTime);
    const currentSlotTime = this.parseTime(slot.startTime);

    const diff =
      (currentSlotTime.getTime() - lastSlotTime.getTime()) / (1000 * 60);

    if (diff === 30) {
      this.selectedSlots.push(slot);
      this.snackBar.open(`Slot ${slot.startTime} - ${slot.endTime} added to selection`, 'Close', {
        duration: 2000,
        panelClass: ['success-snackbar']
      });
    } else {
      this.snackBar.open('You can only select consecutive slots', 'Close', {
        duration: 3000,
        panelClass: ['warning-snackbar']
      });
    }
  }  parseTime(timeStr: string): Date {
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
      this.snackBar.open('No slots selected. Please select at least one slot.', 'Close', {
        duration: 3000,
        panelClass: ['warning-snackbar']
      });
      return;
    }

    if (!this.areSlotsConsecutive()) {
      this.snackBar.open('Selected slots are not consecutive. Please select consecutive slots.', 'Close', {
        duration: 3000,
        panelClass: ['warning-snackbar']
      });
      return;
    }

    this.snackBar.open(`Booking ${this.selectedSlots.length} slots successfully!`, 'Close', {
      duration: 4000,
      panelClass: ['success-snackbar']
    });
    
    // TODO: api call
  }
}
