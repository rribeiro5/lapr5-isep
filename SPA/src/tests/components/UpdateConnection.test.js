import { render, screen } from '@testing-library/react';
import UpdateConnection from '../../components/UpdateConnection/UpdateConnection'
import * as ConnectionService from '../../services/ConnectionService'
import React from "react";
import '../../i18nextInit'

jest.mock('../../services/ConnectionService')

const connection = { destUser: { email: '3@gmail.com' }, connectionStrength: 22, tags: [] }

// Verify connection strength input was rendered
test("render connection strength input", async () => {
    render(<UpdateConnection connection={connection} />)
    const element = screen.getByRole('spinbutton')
    expect(element).toBeInTheDocument()
})

// Verify tags input was rendered
test("render tags input", async () => {
    render(<UpdateConnection connection={connection} />)
    const element = screen.getByRole('textbox')
    expect(element).toBeInTheDocument()
})

// Verify submit button was rendered
test("render submit button input", async () => {
    render(<UpdateConnection connection={connection} />)
    const element = screen.getByRole('button', { value: /Submit/i })
    expect(element).toBeInTheDocument()
})