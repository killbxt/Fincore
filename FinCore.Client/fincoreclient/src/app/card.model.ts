export interface Card {
  id: string;
  cardnumber: string;
  cvc: string;
  clientid: string;
  issuedate: string;
  expirydate: string;
  type: number;
  status: number;
  isdeleted: boolean;
  amount: number;
}
