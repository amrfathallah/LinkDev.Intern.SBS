import { getTestBed } from '@angular/core/testing';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


export interface Resource {
  id: number;
  name: string;
  type: number;
  capacity: number;
  openAt: string;
  closeAt: string;
  active: boolean;
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
      capacity: [1, [Validators.required, Validators.min(1)]],
      openAt: ['09:00', Validators.required],
      closeAt: ['17:00', Validators.required],
      active: [true]
    });
  }

  private populateForm(resource: Resource): void {
    this.resourceForm.patchValue({
      name: resource.name,
      type: resource.type,
      capacity: resource.capacity,
      active: resource.active
    });
  }

  onSave(): void {
    if (this.resourceForm.valid) {
      const formValue = this.resourceForm.value;


      const resourceData: Partial<Resource> = {
        name: formValue.name,
        type: formValue.type === 'desk' ? 2 : 1,
        capacity: formValue.capacity,
        openAt: (formValue.openAt) + ":00",
        closeAt:(formValue.closeAt) + ":00",
        active: formValue.active,
      };

      // Include ID for edit operations
      if (this.data.isEdit && this.data.resource) {
        resourceData.id = this.data.resource.id;
      }
      this.dialogRef.close(resourceData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
