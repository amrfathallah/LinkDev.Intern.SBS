import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

export interface Resource {
  id: number;
  name: string;
  type: 'room' | 'desk';
  location: string;
  capacity: number;
  equipment: string[];
  active: boolean;
  utilizationRate: number;
  nextBooking?: string;
}

export interface ResourceDialogData {
  resource?: Resource;
  isEdit: boolean;
}

@Component({
  selector: 'app-resource-dialog',
  templateUrl: './resource-dialog.component.html',
  styleUrls: ['./resource-dialog.component.css']
})
export class ResourceDialogComponent implements OnInit {
  resourceForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<ResourceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ResourceDialogData,
    private fb: FormBuilder
  ) {
    this.resourceForm = this.createForm();
  }

  ngOnInit(): void {
    if (this.data.isEdit && this.data.resource) {
      this.populateForm(this.data.resource);
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required]],
      type: ['', [Validators.required]],
      location: ['', [Validators.required]],
      capacity: [1, [Validators.required, Validators.min(1)]],
      equipmentText: [''],
      active: [true]
    });
  }

  private populateForm(resource: Resource): void {
    const equipmentText = resource.equipment.join(', ');

    this.resourceForm.patchValue({
      name: resource.name,
      type: resource.type,
      location: resource.location,
      capacity: resource.capacity,
      equipmentText: equipmentText,
      active: resource.active
    });
  }

  onSave(): void {
    if (this.resourceForm.valid) {
      const formValue = this.resourceForm.value;

      // Convert equipment text to array
      const equipment = formValue.equipmentText
        ? formValue.equipmentText.split(',').map((item: string) => item.trim()).filter((item: string) => item)
        : [];

      const resourceData: Partial<Resource> = {
        name: formValue.name,
        type: formValue.type,
        location: formValue.location,
        capacity: formValue.capacity,
        equipment: equipment,
        active: formValue.active,
        utilizationRate: this.data.isEdit ? this.data.resource!.utilizationRate : 0
      };

      // Include ID for edit operations
      if (this.data.isEdit && this.data.resource) {
        resourceData.id = this.data.resource.id;
        resourceData.nextBooking = this.data.resource.nextBooking;
      }

      this.dialogRef.close(resourceData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
