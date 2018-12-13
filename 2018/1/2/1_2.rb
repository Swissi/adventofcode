sum = 0
sums = [sum]
factors = File.readlines("input.txt")
gotone = false


loop do
  factors.each do |factor|
    sum += Integer(factor)
    if sums.include?(sum)
      puts "got one!"
      gotone = true
      puts sum
      break
    else
      sums.push(sum)
    end
  end
  break if gotone == true
end
