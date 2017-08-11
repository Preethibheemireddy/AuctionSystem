CREATE PROCEDURE decide_winnig_bid_place_order
AS
BEGIN

--get the highest bid for the productids that have the bid time in past
select auction.product_id,auction.bid_price, auction.id, auction.customer_id
into #temp_bid_won
from auction,
(select auction.product_id,MAX(auction.bid_price) as CurrentHighestBid from auction
join product on auction.product_id = product.id
where product.product_bid_time < GETDATE()
and product.status_id <> 2
group by auction.product_id) max_result
where max_result.product_id = auction.product_id
and max_result.CurrentHighestBid = auction.bid_price

--set product status inactive so no one can bid
update product 
set status_id = 2, status_reason_id = 3 
from product p
inner join #temp_bid_won tbw
on p.id = tbw.product_id

--update auction table with won status
update auction 
set bidstatus_id = 3, reason_id = 3 
from auction 
inner join #temp_bid_won  
on #temp_bid_won.id = auction.id

--update auction table with lost status
update auction 
set bidstatus_id = 4, reason_id = 4 
from auction 
inner join #temp_bid_won  
on #temp_bid_won.product_id = auction.product_id
where auction.bidstatus_id <> 3

--insert into orders table
insert into [order] ( order_amount, order_datetime, customer_payment_id, auction_id, customer_id, status_id, status_reason_id)
select tbw.bid_price, GETDATE(), cp.id ,tbw.id, tbw.customer_id,3,4
from #temp_bid_won tbw
join customer_payment cp 
on tbw.customer_id = cp.customer_id

END