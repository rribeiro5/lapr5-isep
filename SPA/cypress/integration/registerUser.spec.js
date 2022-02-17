/// <reference types="cypress" />

// test.spec.js created with Cypress
//
// Start writing your Cypress tests below!
// If you're unfamiliar with how Cypress works,
// check out the link below and learn how to write your first test:
// https://on.cypress.io/writing-first-test




import React from 'react';
import { mount } from '@cypress/react';
import App from "../../src/components/App/App";



it('renders learn react link', () => {
    mount(<App />);
    cy.visit('http://localhost:3000/')
});


