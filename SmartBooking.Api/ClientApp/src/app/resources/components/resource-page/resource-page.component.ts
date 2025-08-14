import { Component, Input, OnInit } from '@angular/core';
import { GetResourceDto } from '../../models/dtos/get-resources.dto';
import { ActivatedRoute, Route } from '@angular/router';
import { SlotDto, ComponentSlotDto } from '../../models/dtos/slot.dto';
import { ResourceService } from '../../services/resource-service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BookingRequestDto } from '../../models/dtos/booking-request.dto';
// Interface for component's internal slot representation

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
  slots: ComponentSlotDto[] = [];
  allSlots: SlotDto[] = [];
  selectedSlots: ComponentSlotDto[] = [];
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
        next: (response) => {
          this.resource = response;
          this.loadSlots();
        },
        error: (err) => console.error('Failed to fetch resource', err),
      });
    }
  }

  loadSlots(): void {
    const date = this.selectedDate.toISOString().split('T')[0];

    this.resourceService.getSlots().subscribe({
      next: (slotsResponse) => {
        this.allSlots = slotsResponse.data || [];
        this.resourceService.getBookedSlots(this.id, date).subscribe({
          next: (bookedResponse) => {
            const bookedSlots: SlotDto[] = bookedResponse.bookedSlots.map(
              (slot) => ({
                id: slot.slotId.toString(),
                startTime: slot.startTime.substring(0, 5),
                endTime: slot.endTime.substring(0, 5),
              })
            );

            this.filterSlotsForResource(bookedSlots);
          },
          error: (err) => {
            console.error('Failed to load booked slots:', err);
            console.error('Error details:', err.error);
            this.snackBar.open(
              'Failed to load booked slots. Showing all slots as available.',
              'Close',
              {
                duration: 4000,
                panelClass: ['error-snackbar'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              }
            );
            this.filterSlotsForResource([]);
          },
        });
      },
      error: (err) => {
        console.error('Failed to load slots:', err);
        this.snackBar.open('Failed to load available slots.', 'Close', {
          duration: 4000,
          panelClass: ['error-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      },
    });
  }

  filterSlotsForResource(bookedSlots: SlotDto[]): void {
    this.slots = []; // reset

    const [openHours, openMinutes] = this.resource.openAt
      .split(':')
      .map(Number);
    const [closeHours, closeMinutes] = this.resource.closeAt
      .split(':')
      .map(Number);

    const openTime = new Date();
    openTime.setHours(openHours, openMinutes, 0, 0);

    const closeTime = new Date();
    closeTime.setHours(closeHours, closeMinutes, 0, 0);

    this.slots = this.allSlots
      .filter((slot) => {
        const [slotHours, slotMinutes] = slot.startTime.split(':').map(Number);
        const slotTime = new Date();
        slotTime.setHours(slotHours, slotMinutes, 0, 0);
        return slotTime >= openTime && slotTime < closeTime;
      })
      .map((slot) => {
        const isBooked = bookedSlots.some(
          (bookedSlot) =>
            bookedSlot.startTime === slot.startTime.substring(0, 5)
        );

        return {
          id: slot.id,
          startTime: slot.startTime.substring(0, 5), // Format as HH:MM
          endTime: slot.endTime.substring(0, 5), // Format as HH:MM
          isActive: true,
          isBooked: isBooked,
        } as ComponentSlotDto;
      });

    console.log(
      `Filtered ${this.slots.length} slots for resource operating hours ${this.resource.openAt} - ${this.resource.closeAt}`
    );
  }

  onSlotClick(slot: ComponentSlotDto): void {
    if (!this.resource.isActive || !slot.isActive) {
      this.snackBar.open('This resource is not available', 'Close', {
        duration: 3000,
        panelClass: ['warning-snackbar'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      return;
    }

    if (slot.isBooked) {
      this.snackBar.open(
        `Slot ${slot.startTime} - ${slot.endTime} is already reserved`,
        'Close',
        {
          duration: 3000,
          panelClass: ['error-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      return;
    }

    const index = this.selectedSlots.findIndex(
      (s) => s.startTime === slot.startTime
    );

    if (index > -1) {
      this.selectedSlots.splice(index, 1); // Deselect
      this.snackBar.open(
        `Slot ${slot.startTime} - ${slot.endTime} deselected`,
        'Close',
        {
          duration: 2000,
          panelClass: ['info-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      return;
    }

    if (this.selectedSlots.length === 0) {
      this.selectedSlots.push(slot);
      this.snackBar.open(
        `Slot ${slot.startTime} - ${slot.endTime} selected`,
        'Close',
        {
          duration: 2000,
          panelClass: ['success-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      return;
    }

    const lastSlot = this.selectedSlots[this.selectedSlots.length - 1];
    const lastSlotTime = this.parseTime(lastSlot.startTime);
    const currentSlotTime = this.parseTime(slot.startTime);

    const diff =
      (currentSlotTime.getTime() - lastSlotTime.getTime()) / (1000 * 60);

    if (diff === 60) {
      this.selectedSlots.push(slot);
      this.snackBar.open(
        `Slot ${slot.startTime} - ${slot.endTime} added to selection`,
        'Close',
        {
          duration: 2000,
          panelClass: ['success-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
    } else {
      this.snackBar.open('You can only select consecutive slots', 'Close', {
        duration: 3000,
        panelClass: ['warning-snackbar'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
  }
  parseTime(timeStr: string): Date {
    const [hours, minutes] = timeStr.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return date;
  }

  getSlotIdFromTime(timeStr: string): number {
    const matchingSlot = this.allSlots.find(
      (slot) => slot.startTime.substring(0, 5) === timeStr
    );

    if (matchingSlot) {
      return parseInt(matchingSlot.id);
    }

    const [hours, minutes] = timeStr.split(':').map(Number);
    const slotStartHour = 9;
    const slotStartMinute = 0;

    const totalMinutes =
      hours * 60 + minutes - (slotStartHour * 60 + slotStartMinute);

    // Each slot is 60 minutes, so slot ID = (totalMinutes / 60) + 1
    return Math.floor(totalMinutes / 60) + 1;
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
      if (diff !== 60 * 60 * 1000) return false;
    }

    return true;
  }

  checkout(): void {
    if (this.selectedSlots.length === 0) {
      this.snackBar.open(
        'No slots selected. Please select at least one slot.',
        'Close',
        {
          duration: 3000,
          panelClass: ['warning-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      return;
    }

    if (!this.areSlotsConsecutive()) {
      this.snackBar.open(
        'Selected slots are not consecutive. Please select consecutive slots.',
        'Close',
        {
          duration: 3000,
          panelClass: ['warning-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      return;
    }

    const slotIds = this.selectedSlots.map((slot) =>
      this.getSlotIdFromTime(slot.startTime)
    );

    const bookingRequest: BookingRequestDto = {
      resourceId: this.resource.id,
      date: this.selectedDate.toISOString().split('T')[0], // YYYY-MM-DD format
      slotsIds: slotIds,
    };

    this.snackBar.open('Processing booking...', 'Close', {
      duration: 2000,
      panelClass: ['info-snackbar'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });

    // booking API call
    this.resourceService.bookSlots(bookingRequest).subscribe({
      next: (response) => {
        this.snackBar.open(
          `Successfully booked ${this.selectedSlots.length} slots!`,
          'Close',
          {
            duration: 4000,
            panelClass: ['success-snackbar'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }
        );

        this.selectedSlots = [];
        this.loadSlots(); // Reload
      },
      error: (err) => {
        console.error('Booking failed:', err);
        let errorMessage = 'Booking failed. Please try again.';

        if (err.status === 409) {
          errorMessage =
            'Selected slots are no longer available. Please select different slots.';
        } else if (err.status === 400) {
          errorMessage =
            'Invalid booking request. Please check your selection.';
        } else if (err.status === 500) {
          errorMessage = 'Server error occurred. Please try again later.';
        }

        this.snackBar.open(errorMessage, 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      },
    });
  }
}
